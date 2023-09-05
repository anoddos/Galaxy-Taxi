using System.Linq;
using System.Threading.Tasks;
using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Api.Database.Models;
using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using GalaxyTaxi.Shared.Api.Models.Common;
using GalaxyTaxi.Shared.Api.Models.Filters;
using GalaxyTaxi.Shared.Api.Models.OfficeManagement;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Api.Api
{
	public class OfficeManagementService : IOfficeManagementService
	{
		private readonly Db _db;
		private readonly IHttpContextAccessor _httpContextAccessor;

		public OfficeManagementService(Db db, IHttpContextAccessor httpContextAccessor)
		{
			_db = db;
			_httpContextAccessor = httpContextAccessor;
		}

		public async Task EditOfficeDetails(OfficeInfo request, CallContext context = default)
		{
			if (string.IsNullOrWhiteSpace(request.Address.Name)) throw new ArgumentNullException(nameof(request.Address));
			var office = await _db.Offices.Include(o => o.Address).FirstOrDefaultAsync(o => o.Id == request.OfficeId && o.CustomerCompanyId == GetCompanyId());
			if (office == null)
			{
				throw new InvalidOperationException("Incorrect Office Id");
			}

			MapOfficeRequest(request, office);
			await _db.SaveChangesAsync();
		}

		public async Task<GetOfficesResponse> GetOffices(OfficeManagementFilter? filter = null, CallContext context = default)
		{
			var customerCompanyId = GetCompanyId();
			var offices = await GetOfficesForCustomerAsync(customerCompanyId);

			var response = new GetOfficesResponse
			{
				Offices = offices.Select(office => new OfficeInfo
				{
					OfficeId = office.Id,
					WorkingStartTime = office.WorkingStartTime,
					WorkingEndTime = office.WorkingEndTime,
					Address = new AddressInfo
					{
						Name = office.Address.Name,
						Latitude = office.Address.Latitude,
						Longitude = office.Address.Longitude
					},
					NumberOfEmployees = _db.Employees.Count(e => e.OfficeId == office.Id)
				}).ToList()
			};

			return response;
		}

		public async Task<OfficeInfo> AddOffice(OfficeInfo request, CallContext context = default)
		{
			var office = new Office();
			office.Address = new Address();
			MapOfficeRequest(request, office);

			await _db.Offices.AddAsync(office);
			await _db.SaveChangesAsync();
			request.OfficeId = office.Id;
			request.Address.Id = office.AddressId;
			return request;
		}


		private void MapOfficeRequest(OfficeInfo request, Office office)
		{
			var customerCompanyId = GetCompanyId();
			if (customerCompanyId == -1)
			{
				throw new Exception();
			}

			if (string.IsNullOrWhiteSpace(request.Address.Name)) throw new ArgumentNullException(nameof(request.Address));

			office.CustomerCompanyId = customerCompanyId;
			office.Address.Name = request.Address.Name;
			office.Address.Longitude = (double)request.Address.Longitude;
			office.Address.Latitude = (double)request.Address.Latitude;
			office.WorkingStartTime = request.WorkingStartTime;
			office.WorkingEndTime = request.WorkingEndTime;
		}

		private async Task<List<Office>> GetOfficesForCustomerAsync(long customerCompanyId)
		{
			return await _db.Offices
				.Include(o => o.Address)
				.Where(o => o.CustomerCompanyId == customerCompanyId)
				.ToListAsync();
		}

		private long GetCompanyId()
		{
			var httpContext = _httpContextAccessor.HttpContext;
			var res = httpContext?.User.Claims.FirstOrDefault(c => c.Type == AuthenticationKey.CompanyId);
			return long.Parse(res.Value ?? "-1");
		}
	}
}
