using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace GalaxyTaxi.Shared.Api.Interfaces;

[Service("EmployeeManagement")]
public interface IEmployeeManagementService
{
    Task AddEmployees(AddEmployeesRequest request, CallContext context = default);
    Task EditEmployeeDetails(AddEmployeesRequest request, CallContext context = default);
    Task DeleteEmployee(DeleteEmployeeRequest request, CallContext context = default);
}