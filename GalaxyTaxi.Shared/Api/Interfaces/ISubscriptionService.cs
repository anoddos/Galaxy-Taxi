using GalaxyTaxi.Shared.Api.Models.Subscription;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace GalaxyTaxi.Shared.Api.Interfaces;

[Service("ISubscriptionService")]
public interface ISubscriptionService
{
    Task ChoseSubscriptionType(SubscriptionRequest request, CallContext context = default);
    Task<GetSubscriptionDetailResponse> GetSubscriptionDetails(CallContext context = default);
    Task UpdateSubscriptionStatus();
}