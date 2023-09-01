using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Api.Database.Models;
using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using GalaxyTaxi.Shared.Api.Models.Common;
using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using GalaxyTaxi.Shared.Api.Models.JourneyGenerator;
using GalaxyTaxi.Shared.Api.Models.OfficeManagement;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Api.Api;

public class JourneyGeneratorService : IJourneyGeneratorService
{
    private readonly Db _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JourneyGeneratorService(Db db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public async Task<GenerateJourneysResponse> GenerateRoutesForCompany(GenerateJourneysRequest request, CallContext context = default)
    {
        var companyId = GetCompanyId();

        var offices = await _db.Offices.Include(x => x.Address).Where(x => x.CustomerCompanyId == companyId).ToListAsync();
        
        var journeys = new List<Journey>();

        foreach (var office in offices)
        {
            var officeEmployeesWithoutJourneys = await _db.Employees
                .Include(x => x.Addresses)
                .ThenInclude(x => x.Address)
                .Where(x => x.CustomerCompanyId == companyId && !x.HasActiveJourney && x.Addresses.Any(xx => xx.IsActive && xx.Address.IsDetected)).ToListAsync();
            
            journeys.AddRange(await GenerateJourneysForEmployees(companyId, office.Address, officeEmployeesWithoutJourneys));
        }
        
        
        return new GenerateJourneysResponse
        {
            Journeys = journeys.Select(x => new JourneyInfo
            {
                Office = new OfficeInfo
                {
                    OfficeId = x.OfficeId,
                    WorkingEndTime = x.Office.WorkingEndTime,
                    WorkingStartTime = x.Office.WorkingStartTime,
                    Address = new AddressInfo
                    {
                        Id = x.Office.Address.Id,
                        Name = x.Office.Address.Name,
                        Latitude = x.Office.Address.Latitude,
                        Longitude = x.Office.Address.Longitude
                    }
                },
                Stops = x.Stops.Select(xx => new StopInfo
                {
                    Id = xx.Id,
                    Address = new AddressInfo
                    {
                        Id = xx.EmployeeAddress.Address.Id,
                        Name = xx.EmployeeAddress.Address.Name,
                        Latitude = xx.EmployeeAddress.Address.Latitude,
                        Longitude = xx.EmployeeAddress.Address.Longitude
                    },
                    EmployeeDetails = new SingleEmployeeInfo
                    {
                        Mobile = xx.EmployeeAddress.Employee.Mobile,
                        FirstName = xx.EmployeeAddress.Employee.FirstName,
                        LastName = xx.EmployeeAddress.Employee.LastName
                    }
                })
            }).ToList()
        };
    }

    private async Task<List<Journey>> GenerateJourneysForEmployees(long companyId, Address officeAddress, List<Employee> companyEmployeesWithoutJourneys)
    {
        var result = new List<Journey>();
        
    /*
        var employeeLocations = new List<Tuple<decimal, decimal>>();

        foreach (var employee in companyEmployeesWithoutJourneys)
        {
            var address = employee.Addresses.Single(x => x.IsActive && x.Address.IsDetected).Address;
            employeeLocations.Add(Tuple.Create(address.Latitude, address.Latitude));
        }
        
        var officeLocation = Tuple.Create(officeAddress.Latitude, officeAddress.Longitude);  // Office location
     */   
        result.Add( new Journey
        {
            CustomerCompanyId = companyId,
            OfficeId = companyEmployeesWithoutJourneys.First().OfficeId,
            Stops = new List<Stop>
            {
                new Stop
                {
                    EmployeeAddressId = companyEmployeesWithoutJourneys.First().Addresses.Single(x => x.IsActive && x.Address.IsDetected).Id
                },
                new Stop
                {
                    EmployeeAddressId = companyEmployeesWithoutJourneys[1].Addresses.Single(x => x.IsActive && x.Address.IsDetected).Id
                },
                new Stop
                {
                    EmployeeAddressId = companyEmployeesWithoutJourneys[2].Addresses.Single(x => x.IsActive  && x.Address.IsDetected).Id
                }
            }
        });

        await Task.CompletedTask;
        
        return result;
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