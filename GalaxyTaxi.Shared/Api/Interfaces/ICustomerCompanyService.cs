using GalaxyTaxi.Shared.Api.Models.CustomerCompany;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Shared.Api.Interfaces;

public interface ICustomerCompanyService
{
    Task SaveMaxAmountPerEmployee(SaveMaxAmountPerEmployeeRequest request, CallContext context = default);
    
    Task PauseSubscription(PauseSubscriptionRequest request, CallContext context = default);
    
    Task CancelSubscription(PauseSubscriptionRequest request, CallContext context = default);
}