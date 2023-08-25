using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Api.Database.Models;
using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using GalaxyTaxi.Shared.Api.Models.Common;
using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using GalaxyTaxi.Shared.Api.Models.Filters;
using Microsoft.EntityFrameworkCore;
using ProtoBuf;
using ProtoBuf.Grpc;
using System.Linq;

namespace GalaxyTaxi.Api.Api;

public class EmployeeManagementService : IEmployeeManagementService
{
    private readonly Db _db;
    private readonly IAddressDetectionService _addressDetectionService;

    public EmployeeManagementService(Db db, IAddressDetectionService addressDetectionService)
    {
        _db = db;
        _addressDetectionService = addressDetectionService;
    }

    public async Task AddEmployees(AddEmployeesRequest request, CallContext context = default)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        if (request.employeesInfo == null || request.employeesInfo.Count == 0) return;
        try
        {

            foreach (var employeeinfo in request.employeesInfo)
            {
                // validate officeid and companyid
                DetectAddressCoordinatesResponse addressRes = await ValidateEmployeeAndGetAddress(employeeinfo);

                var address = _db.Addresses.FirstOrDefault(a => a.Latitude == addressRes.Lat & a.Longitude == addressRes.Long);
                if (address == null)
                {
                    address = new Address { Name = employeeinfo.Address, Latitude = addressRes.Lat, Longitude = addressRes.Long };
                    await _db.Addresses.AddAsync(address);
                    await _db.SaveChangesAsync();
                }


                // validate mobile uniqueness
                var existingemployee = await _db.Employees.FirstOrDefaultAsync(e => e.Mobile == employeeinfo.Mobile);

                if (existingemployee == null)
                {
                    // Employee doesn't exist, insert a new employee
                    existingemployee = new Employee
                    {
                        FirstName = employeeinfo.FirstName,
                        LastName = employeeinfo.LastName,
                        Mobile = employeeinfo.Mobile,
                        CustomerCompanyId = employeeinfo.CustomerCompanyId,
                        OfficeId = employeeinfo.OfficeId,
                        Addresses = new List<EmployeeAddress>
                        {
                            new EmployeeAddress
                            {
                                Address = address,
                                IsActive = true
                            }
                        }
                    };

                    _db.Employees.Add(existingemployee);
                }
                else
                {
                    // Employee exists, update officeId, mobile and check address
                    existingemployee.OfficeId = employeeinfo.OfficeId;
                    existingemployee.Mobile = employeeinfo.Mobile;
                    var existingAddress = existingemployee.Addresses.FirstOrDefault(a => a.AddressId == address.Id);

                    if (existingAddress == null)
                    {
                        existingemployee.Addresses.Add(new EmployeeAddress
                        {
                            Employee = existingemployee,
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




    private async Task<DetectAddressCoordinatesResponse> ValidateEmployeeAndGetAddress(SingleEmployeeInfo employeeInfo)
    {
        if (string.IsNullOrEmpty(employeeInfo.FirstName))
        {
            throw new InvalidOperationException($"first name cannot be empty");
        }

        if (string.IsNullOrEmpty(employeeInfo.LastName))
        {
            throw new InvalidOperationException($"last name cannot be empty");
        }

        if (string.IsNullOrEmpty(employeeInfo.Mobile))
        {
            throw new InvalidOperationException($"Mobile cannot be empty");
        }

        if (string.IsNullOrEmpty(employeeInfo.Address))
        {
            throw new InvalidOperationException($"Address cannot be empty");
        }
        var officeexists = await _db.Offices.AnyAsync(o => o.Id == employeeInfo.OfficeId);
        var companyexists = await _db.CustomerCompanies.AnyAsync(cc => cc.Id == employeeInfo.CustomerCompanyId);

        if (!officeexists || !companyexists)
        {
            throw new InvalidOperationException($"invalid officeid {employeeInfo.OfficeId} or companyid {employeeInfo.CustomerCompanyId}");
        }

        if (string.IsNullOrEmpty(employeeInfo.Address))
        {
            throw new InvalidOperationException($"address cant be empty for employee, mobile - {employeeInfo.Mobile}");
        }

        DetectAddressCoordinatesResponse addressRes =
                    //await _addressDetectionService.DetectAddressCoordinates(
                    // new DetectAddressCoordinatesRequest { Address = employeeinfo.Address });
                    new DetectAddressCoordinatesResponse { Lat = 1111, Long = 2222, StatusId = ActionStatus.Success };

        if (addressRes == null)
        {
            throw new InvalidOperationException($"address does not exist - {employeeInfo.Address}");

        }
        return addressRes;

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
        if (filter == null) return null;

        var employees = from employee in _db.Employees.Include(x => x.Office)
                   where employee.CustomerCompanyId == filter.CustomerCompanyId
                   let address = employee.Addresses.First(a => a.IsActive)
                   select  new EmployeeJourneyInfo
                   {
                       FirstName = employee.FirstName,
                       LastName = employee.LastName,
                       Mobile = employee.Mobile,
                       From = new EmployeeAddressInfo
                       {
                           Name = address.Address.Name,
                           Latitude = address.Address.Latitude,
                           Longitude = address.Address.Longitude
                       },
                       To = new EmployeeAddressInfo 
                       { 
                           Name = employee.Office.Address.Name,
                           Latitude = employee.Office.Address.Latitude,
                           Longitude = employee.Office.Address.Longitude
                       }
                   };

        return new GetEmployeesResponse { Employees = await employees.ToListAsync()};

    }

}