using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using GalaxyTaxi.Shared.Api.Models.Filters;
using ProtoBuf.Grpc;
using System.Net.Http;

namespace GalaxyTaxi.Api.Api;

public class EmployeeManagementService : IEmployeeManagementService
{

    private readonly Db _db;

    public EmployeeManagementService(Db db)
    {
        _db = db;
    }

    public Task AddEmployees(AddEmployeesRequest request, CallContext context = default)
    {
             if (request == null) throw new ArgumentNullException(nameof(request));
        if (request.employeesInfo == null || request.employeesInfo.Count == 0) return Task.CompletedTask;

        Console.WriteLine("to implement");
        //implement insert here
        throw new NotImplementedException();
    }

    public Task EditEmployeeDetails(AddEmployeesRequest request, CallContext context = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteEmployee(DeleteEmployeeRequest request, CallContext context = default)
    {
        throw new NotImplementedException();
    }

    public Task<GetEmployeesResponse> GetEmployees(EmployeeManagementFilter? filter = null, CallContext context = default)
    {
        throw new NotImplementedException();
    }

}