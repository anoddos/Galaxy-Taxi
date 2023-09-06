using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Api.Database.Models;
using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using GalaxyTaxi.Shared.Api.Models.Common;
using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using GalaxyTaxi.Shared.Api.Models.Filters;
using GalaxyTaxi.Shared.Api.Models.OfficeManagement;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace GalaxyTaxi.Api.Api;

public class EmployeeManagementService : IEmployeeManagementService
{
    private readonly Db _db;
    private readonly IAddressDetectionService _addressDetectionService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public EmployeeManagementService(Db db, IAddressDetectionService addressDetectionService,
        IHttpContextAccessor httpContextAccessor)
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
        var subscription = await _db.Subscriptions.FirstOrDefaultAsync(s =>
            s.CustomerCompanyId == customerCompanyId && s.SubscriptionStatus == SubscriptionStatus.Active);

        if (subscription == null)
            throw new RpcException(new Status(StatusCode.OutOfRange, "Please Buy Subscription First"));

        var numberOfEmployees = await _db.Employees.CountAsync(e => e.CustomerCompanyId == customerCompanyId);

        var additionalCounter = 0;

        try
        {
            var employees = new List<Employee>();
            foreach (var employeeInfo in request.EmployeesInfo)
            {
                try
                {
                    await ValidateEmployee(employeeInfo, customerCompanyId);

                    var address = new Address { Name = employeeInfo.Address };
                    await _db.Addresses.AddAsync(address);
                    await _db.SaveChangesAsync();

                    // validate mobile uniqueness
                    var existingEmployee = await _db.Employees.FirstOrDefaultAsync(e => e.Mobile == employeeInfo.Mobile && e.CustomerCompanyId == customerCompanyId);

                    var employeeExists = existingEmployee != null;
                    var isAddressUpdated = false;
                    
                    var office = await _db.Offices.SingleAsync(o => o.CustomerCompanyId == customerCompanyId && o.OfficeIdentification == employeeInfo.OfficeId);

                    if (!employeeExists)
                    {
                        if (numberOfEmployees + additionalCounter >= SubscriptionMapping.Mapping[subscription.SubscriptionPlanTypeId])
                        {
                            throw new RpcException(new Status(StatusCode.OutOfRange, "Can not add any more employees"));
                        }

                        existingEmployee = new Employee
                        {
                            FirstName = employeeInfo.FirstName,
                            LastName = employeeInfo.LastName,
                            Mobile = employeeInfo.Mobile,
                            CustomerCompanyId = customerCompanyId,
                            OfficeId = office.Id,
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
                        existingEmployee.OfficeId = office.Id;
                        existingEmployee.Mobile = employeeInfo.Mobile;
                        var deactivateAddress = _db.EmployeeAddresses.FirstOrDefault(ea => ea.EmployeeId == existingEmployee.Id);
                        if (deactivateAddress != null)
                        {
                            deactivateAddress.IsActive = false;
                        }

                        var existingAddress = existingEmployee.Addresses.FirstOrDefault(a => a.AddressId == address.Id && a.EmployeeId == existingEmployee.Id);

                        if (existingAddress == null)
                        {
                            existingEmployee.Addresses.Add(new EmployeeAddress
                            {
                                Employee = existingEmployee,
                                Address = address,
                                IsActive = true
                            });
                        }
                        else
                        {
                            if (existingAddress.Address.Name != address.Name || !existingAddress.Address.IsDetected)
                            {
                                isAddressUpdated = true;
                                existingAddress.Address.Name = address.Name;
                            }
                        }
                    }

                    await _db.SaveChangesAsync();
                    
                    additionalCounter++;
                    if (!employeeExists || !isAddressUpdated)
                    {
                        var detectionResult = await _addressDetectionService.DetectSingleAddressCoordinates(new DetectAddressCoordinatesRequest { EmployeeId = existingEmployee.Id });
                        address.IsDetected = detectionResult?.StatusId == AddressDetectionStatus.Success;

                        await _db.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    //implement
                }
            }
            
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task EditEmployeeDetails(EmployeeJourneyInfo request, CallContext context = default)
    {
        var customerCompanyId = long.Parse(GetSessionValue(AuthenticationKey.CompanyId) ?? "-1");

        var employee = await _db.Employees.FirstOrDefaultAsync(o =>
            o.Id == request.EmployeeId && o.CustomerCompanyId == customerCompanyId);
        if (employee == null)
        {
            throw new InvalidOperationException("Incorrect employee Id");
        }

        employee.FirstName = request.FirstName;
        employee.LastName = request.LastName;
        employee.Mobile = request.Mobile;
        employee.OfficeId = request.To.OfficeId;
        var address = _db.Addresses.SingleOrDefault(a => a.Id == request.From.Id);
        address.Name = request.From.Name;
        try
        {
            var response = await _addressDetectionService.DetectAddressCoordinatesFromName(request.From);
            address.Latitude = (double)response.Latitude;
            address.Longitude = (double)response.Longitude;
            address.IsDetected = true;
        }
        catch (Exception ex)
        {
            address.IsDetected = false;
        }

        await _db.SaveChangesAsync();
    }

    public async Task DeleteEmployee(DeleteEmployeeRequest request, CallContext context = default)
    {
        var customerCompanyId = long.Parse(GetSessionValue(AuthenticationKey.CompanyId) ?? "-1");

        var employee = await _db.Employees
            .Include(e => e.Addresses)
            .FirstOrDefaultAsync(o => o.Id == request.EmployeeId && o.CustomerCompanyId == customerCompanyId);

        if (employee != null)
        {
            _db.EmployeeAddresses.RemoveRange(employee.Addresses);
            _db.Employees.Remove(employee);

            await _db.SaveChangesAsync();
        }
    }

    public async Task<GetEmployeesResponse> GetEmployees(EmployeeManagementFilter? filter = null,
        CallContext context = default)
    {
        var customerCompanyId = long.Parse(GetSessionValue(AuthenticationKey.CompanyId) ?? "-1");

        var employees = from employee in _db.Employees.Include(e => e.Office)
            where (employee.CustomerCompanyId == customerCompanyId
                   && (filter.JourneyStatus == EmployeeJourneyStatus.All
                       || (filter.JourneyStatus == EmployeeJourneyStatus.HasActiveJourney && employee.HasActiveJourney)
                       || (filter.JourneyStatus == EmployeeJourneyStatus.NoActiveJourney &&
                           !employee.HasActiveJourney)))
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
                    Longitude = address.Address.Longitude,
                    Id = address.Address.Id,
                    IsDetected = address.Address.IsDetected
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
                        Longitude = employee.Office.Address.Longitude,
                        IsDetected = employee.Office.Address.IsDetected
                    }
                },
                HasActiveJourney = employee.HasActiveJourney
            };

        if (filter?.SelectedOffice != null)
        {
            employees = employees.Where(e => e.To.OfficeId == filter.SelectedOffice.OfficeId);
        }

        if (!string.IsNullOrWhiteSpace(filter?.EmployeeName))
        {
            string[]? names = filter?.EmployeeName.Trim()
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (names != null)
            {
                foreach (string namePart in names)
                {
                    employees = employees.Where(e =>
                        EF.Functions.Like(e.FirstName, $"%{namePart}%") ||
                        EF.Functions.Like(e.LastName, $"%{namePart}%")
                    );
                }
            }
        }

        return new GetEmployeesResponse { Employees = await employees?.ToListAsync() };
    }

    private async Task ValidateEmployee(SingleEmployeeInfo employeeInfo, long customerCompanyId)
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

        var officeExists = await _db.Offices.AnyAsync(o => o.OfficeIdentification == employeeInfo.OfficeId && o.CustomerCompanyId == customerCompanyId);
        var mobileExists = await _db.Employees.AnyAsync(e => e.Mobile == employeeInfo.Mobile && e.CustomerCompanyId != customerCompanyId);
        
        if (mobileExists)
        {
            throw new InvalidOperationException(
                $"employee with the same mobile already exists in different company {employeeInfo.Mobile}");
        }

        if (!officeExists)
        {
            throw new InvalidOperationException($"invalid officeId {employeeInfo.OfficeId}");
        }

        if (string.IsNullOrEmpty(employeeInfo.Address))
        {
            throw new InvalidOperationException($"address cant be empty for employee, mobile - {employeeInfo.Mobile}");
        }
    }

    private string GetSessionValue(string key)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var res = httpContext?.User.Claims.FirstOrDefault(c => c.Type == key);

        return res == null ? "" : res.Value;
    }
}