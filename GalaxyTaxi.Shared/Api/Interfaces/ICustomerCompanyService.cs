using GalaxyTaxi.Shared.Api.Models.CustomerCompany;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace GalaxyTaxi.Shared.Api.Interfaces;

[Service("CustomerCompany")]
public interface ICustomerCompanyService
{
    Task SaveMaxAmountPerEmployee(SaveMaxAmountPerEmployeeRequest request, CallContext context = default);
}