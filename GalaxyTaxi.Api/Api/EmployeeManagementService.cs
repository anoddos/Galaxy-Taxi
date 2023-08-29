using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Api.Database.Models;
using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using GalaxyTaxi.Shared.Api.Models.Common;
using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using GalaxyTaxi.Shared.Api.Models.Filters;
using GalaxyTaxi.Shared.Api.Models.OfficeManagement;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Api.Api;

public class EmployeeManagementService : IEmployeeManagementService
{
    private readonly Db _db;
    private readonly IAddressDetectionService _addressDetectionService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EmployeeManagementService(Db db, IAddressDetectionService addressDetectionService, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _addressDetectionService = addressDetectionService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task AddEmployees(AddEmployeesRequest request, CallContext context = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        if (request.EmployeesInfo == null || request.EmployeesInfo.Count == 0) return;

        var customerCompanyId = long.Parse(GetSessionValue(AuthenticationKey.CompanyId) ?? "-1");

        try
        {
            foreach (var employeeInfo in request.EmployeesInfo)
            {
                var addressRes = await ValidateEmployeeAndGetAddress(employeeInfo, customerCompanyId);

                var address = _db.Addresses.FirstOrDefault(a => a.Latitude == addressRes.Lat & a.Longitude == addressRes.Long);
                if (address == null)
                {
                    address = new Address { Name = employeeInfo.Address, Latitude = addressRes.Lat, Longitude = addressRes.Long };
                    await _db.Addresses.AddAsync(address);
                    await _db.SaveChangesAsync();
                }

                // validate mobile uniqueness
                var existingEmployee = await _db.Employees.FirstOrDefaultAsync(e => e.Mobile == employeeInfo.Mobile);

                if (existingEmployee == null)
                {
                    existingEmployee = new Employee
                    {
                        FirstName = employeeInfo.FirstName,
                        LastName = employeeInfo.LastName,
                        Mobile = employeeInfo.Mobile,
                        CustomerCompanyId = customerCompanyId,
                        OfficeId = employeeInfo.OfficeId,
                        Addresses = new List<EmployeeAddress>
                        {
                            new()
                            {
                                Address = address,
                                IsActive = true
                            }
                        }
                    };

                    _db.Employees.Add(existingEmployee);
                }
                else
                {
                    // Employee exists, update officeId, mobile and check address
                    existingEmployee.OfficeId = employeeInfo.OfficeId;
                    existingEmployee.Mobile = employeeInfo.Mobile;
                    var existingAddress = existingEmployee.Addresses.FirstOrDefault(a => a.AddressId == address.Id);

                    if (existingAddress == null)
                    {
                        existingEmployee.Addresses.Add(new EmployeeAddress
                        {
                            Employee = existingEmployee,
                            Address = address
                        });
                    }
                }
            }

            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public Task EditEmployeeDetails(AddEmployeesRequest request, CallContext context = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteEmployee(DeleteEmployeeRequest request, CallContext context = default)
    {
        throw new NotImplementedException();
    }

    public async Task<GetEmployeesResponse> GetEmployees(EmployeeManagementFilter? filter = null, CallContext context = default)
    {
        //to implement in more detail, for testing purposes now

        var customerCompanyId = long.Parse(GetSessionValue(AuthenticationKey.CompanyId) ?? "-1");

        var employees = from employee in _db.Employees.Include(e => e.Office)
                        where employee.CustomerCompanyId == customerCompanyId
                        let address = employee.Addresses.SingleOrDefault(a => a.IsActive)
                        select new EmployeeJourneyInfo
                        {
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            Mobile = employee.Mobile,
                            EmployeeId = employee.Id,
                            From = new AddressInfo
                            {
                                Name = address.Address.Name,
                                Latitude = address.Address.Latitude,
                                Longitude = address.Address.Longitude
                            },
                            To = new OfficeInfo
                            {
                                OfficeId = employee.OfficeId,
                                WorkingEndTime = employee.Office.WorkingEndTime,
                                WorkingStartTime = employee.Office.WorkingStartTime,
                                Address = new AddressInfo
                                {
                                    Name = employee.Office.Address.Name,
                                    Latitude = employee.Office.Address.Latitude,
                                    Longitude = employee.Office.Address.Longitude
                                }
                            }
                        };

        if (filter?.SelectedOffice != null)
        {
            employees = employees.Where(e => e.To.OfficeId == filter.SelectedOffice.OfficeId);
        }

        return new GetEmployeesResponse { Employees = await employees.ToListAsync() };
    }

    private async Task<DetectAddressCoordinatesResponse> ValidateEmployeeAndGetAddress(SingleEmployeeInfo employeeInfo, long customerCompanyId)
    {
        if (string.IsNullOrEmpty(employeeInfo.FirstName))
        {
            throw new InvalidOperationException("first name cannot be empty");
        }

        if (string.IsNullOrEmpty(employeeInfo.LastName))
        {
            throw new InvalidOperationException("last name cannot be empty");
        }

        if (string.IsNullOrEmpty(employeeInfo.Mobile))
        {
            throw new InvalidOperationException("Mobile cannot be empty");
        }

        if (string.IsNullOrEmpty(employeeInfo.Address))
        {
            throw new InvalidOperationException("Address cannot be empty");
        }
        var officeExists = await _db.Offices.AnyAsync(o => o.Id == employeeInfo.OfficeId);
        var companyExists = await _db.CustomerCompanies.AnyAsync(cc => cc.Id == customerCompanyId);

        if (!officeExists || !companyExists)
        {
            throw new InvalidOperationException($"invalid officeId {employeeInfo.OfficeId} or companyId {customerCompanyId}");
        }

        if (string.IsNullOrEmpty(employeeInfo.Address))
        {
            throw new InvalidOperationException($"address cant be empty for employee, mobile - {employeeInfo.Mobile}");
        }

        var addressRes =
            //await _addressDetectionService.DetectAddressCoordinates(
            // new DetectAddressCoordinatesRequest { Address = employeeinfo.Address });
            new DetectAddressCoordinatesResponse { Lat = 1111, Long = 2222, StatusId = ActionStatus.Success };

        if (addressRes == null)
        {
            throw new InvalidOperationException($"address does not exist - {employeeInfo.Address}");

        }

        return addressRes;
    }

    private string GetSessionValue(string key)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var res = httpContext?.User.Claims.FirstOrDefault(c => c.Type == key);

        return res == null ? "" : res.Value;
    }
}