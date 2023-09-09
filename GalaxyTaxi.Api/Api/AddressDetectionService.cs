using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Api.Database.Models;
using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using GalaxyTaxi.Shared.Api.Models.Common;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using ProtoBuf.Grpc;
using Stripe;
using System.Globalization;

namespace GalaxyTaxi.Api.Api;

public class AddressDetectionService : IAddressDetectionService
{
	private readonly Db _db;
	private readonly IHttpContextAccessor _httpContextAccessor;
	private readonly IConfiguration _config;
	public AddressDetectionService(Db db, IHttpContextAccessor httpContextAccessor, IConfiguration config)
	{
		_db = db;
		_httpContextAccessor = httpContextAccessor;
		_config = config;
	}

	public async Task<DetectAddressCoordinatesResponse> DetectSingleAddressCoordinates(DetectAddressCoordinatesRequest request, CallContext context = default)
	{
		var companyId = GetCompanyId();

		var employee = _db.Employees.SingleOrDefault(x => x.Id == request.EmployeeId && x.CustomerCompanyId == companyId);

		if (employee == null)
		{
			throw new RpcException(new Status(StatusCode.NotFound, "Employee Not Found"));
		}

		var lst = new List<Employee> { employee };
		var address = (await DetectAddressCoordinatesForEmployees(lst)).First();

		return new DetectAddressCoordinatesResponse
		{
			Lat = address.Latitude,
			Long = address.Longitude,
			StatusId = address.IsDetected ? AddressDetectionStatus.Success : AddressDetectionStatus.Fail
		};
	}

	public async Task<DetectCoordinatesForCompanyEmployeesResponse> DetectCoordinatesForCompanyEmployees(DetectCoordinatesForCompanyEmployeesRequest request, CallContext context = default)
	{
		var companyId = GetCompanyId();

		var employees = await _db.Employees.Include(x => x.Addresses).ThenInclude(x => x.Address).Where(x => x.CustomerCompanyId == companyId && !x.Addresses.Single(xx => xx.IsActive).Address.IsDetected).ToListAsync();

		var addresses = await DetectAddressCoordinatesForEmployees(employees);

		return new DetectCoordinatesForCompanyEmployeesResponse
		{
			DetectResponses = addresses.Select(x => new DetectAddressCoordinatesResponse
			{
				Lat = x.Latitude,
				Long = x.Longitude,
				StatusId = x.IsDetected ? AddressDetectionStatus.Success : AddressDetectionStatus.Fail
			}).ToList()
		};
	}

	private async Task<IEnumerable<Database.Models.Address>> DetectAddressCoordinatesForEmployees(IEnumerable<Employee> employees)
	{
		var result = new List<Database.Models.Address>();
		var apiKey = _config.GetValue<string>("GoogleMapsKey"); 

		using (var client = new HttpClient())
		{
			foreach (var employee in employees)
			{
				var address = employee.Addresses.Single(x => x.IsActive).Address;
				var locationName = address.Name;

				var apiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(locationName)}&key={apiKey}";

				var response = await client.GetAsync(apiUrl);

				if (response.IsSuccessStatusCode)
				{
					var responseContent = await response.Content.ReadAsStringAsync();
					var jsonResponse = JObject.Parse(responseContent);

					var location = jsonResponse["results"][0]["geometry"]["location"];
					var latitude = (double)location["lat"];
					var longitude = (double)location["lng"];

					address.Latitude = latitude;
					address.Longitude = longitude;
					address.IsDetected = true;
				}
				else
				{
					address.IsDetected = false;
				}

				result.Add(address);
			}
		}

		return result;
	}

	public async Task<AddressInfo> DetectAddressNameFromCoordinates(AddressInfo detectAddress)
	{
		var apiKey = _config.GetValue<string>("GoogleMapsKey");

		using (var client = new HttpClient())
		{
			var latitude = detectAddress.Latitude?.ToString("0.###############", CultureInfo.InvariantCulture);
			var longtitude = detectAddress.Longitude?.ToString("0.###############", CultureInfo.InvariantCulture);
			var apiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?latlng={latitude},{longtitude}&key={apiKey}";

			var response = await client.GetAsync(apiUrl);

			if (response.IsSuccessStatusCode)
			{
				string jsonResponse = await response.Content.ReadAsStringAsync();
				JObject data = JObject.Parse(jsonResponse);
				string formattedAddress = (string)data["results"][0]["formatted_address"];
				detectAddress.Name = formattedAddress;
			}
		}

		return detectAddress;
	}

	public async Task<AddressInfo> DetectAddressCoordinatesFromName(AddressInfo detectAddress)
	{
		var apiKey = _config.GetValue<string>("GoogleMapsKey");

		try
		{
			using (var client = new HttpClient())
			{
				var apiUrl = $"https://maps.googleapis.com/maps/api/geocode/json?address={Uri.EscapeDataString(detectAddress.Name)}&key={apiKey}";

				var response = await client.GetAsync(apiUrl);

				if (response.IsSuccessStatusCode)
				{
					var responseContent = await response.Content.ReadAsStringAsync();
					var jsonResponse = JObject.Parse(responseContent);
						
					var location = jsonResponse["results"][0]["geometry"]["location"];
					var latitude = (double)location["lat"];
					var longitude = (double)location["lng"];

					detectAddress.Latitude = latitude;
					detectAddress.Longitude = longitude;

					detectAddress.IsDetected = true;
				}
			}
		}
		catch (Exception e)
		{
			return new AddressInfo
			{
				IsDetected = false
			};
		}

		return detectAddress;
	}

	private string GetSessionValue(string key, CallContext context = default)
	{
		var httpContext = _httpContextAccessor.HttpContext;
		var res = httpContext?.User.Claims.FirstOrDefault(c => c.Type == key);

		return res == null ? "" : res.Value;
	}

	private long GetCompanyId()
	{
		var companyId = GetSessionValue(AuthenticationKey.CompanyId);

		if (string.IsNullOrWhiteSpace(companyId))
		{
			throw new RpcException(new Status(StatusCode.NotFound, "Not Logged In"));
		}

		return long.Parse(companyId);
	}
}