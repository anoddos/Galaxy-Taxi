using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using GalaxyTaxi.Shared.Api.Models.Filters;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace GalaxyTaxi.Shared.Api.Interfaces;

[Service("EmployeeManagement")]
public interface IEmployeeManagementService
{
    Task<AddEmployeeResponse> AddEmployees(AddEmployeesRequest request, CallContext context = default);
    Task EditEmployeeDetails(EmployeeJourneyInfo request, CallContext context = default);
    Task DeleteEmployee(DeleteEmployeeRequest request, CallContext context = default);
    Task<GetEmployeesResponse> GetEmployees(EmployeeManagementFilter? filter = null, CallContext context = default);
}