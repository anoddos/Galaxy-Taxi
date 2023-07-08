using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Shared.Api.Interfaces;

public interface IEmployeeManagementService
{
    Task AddEmployees(AddEmployeesRequest request, CallContext context = default);
    Task EditEmployeeDetails(AddEmployeesRequest request, CallContext context = default);
    Task DeleteEmployee(DeleteEmployeeRequest request, CallContext context = default);
}