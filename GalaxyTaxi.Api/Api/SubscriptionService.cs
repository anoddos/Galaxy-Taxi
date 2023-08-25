using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.Subscription;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Api.Api;

public class SubscriptionService : ISubscriptionService
{
    public async Task ChoseSubscriptionType(SubscriptionRequest request, CallContext context = default)
    {
        var completedTask = Task.CompletedTask;
    }
}