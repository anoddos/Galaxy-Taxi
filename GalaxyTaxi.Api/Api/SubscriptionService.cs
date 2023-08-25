using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Api.Database.Models;
using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.Common;
using GalaxyTaxi.Shared.Api.Models.Subscription;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Api.Api;

public class SubscriptionService : ISubscriptionService
{
    private readonly Db _db;

    private readonly Dictionary<SubscriptionPlanType, decimal> _prices = new()
    {
        { SubscriptionPlanType.Weekly, 7m },
        { SubscriptionPlanType.Monthly, 20m },
        { SubscriptionPlanType.Annual, 200m }
    };

    public SubscriptionService(Db db)
    {
        _db = db;
    }

    public async Task ChoseSubscriptionType(SubscriptionRequest request, CallContext context = default)
    {
        var subscriptionInDb = await _db.Subscriptions.SingleOrDefaultAsync(x => x.CustomerCompanyId == 13);

        if (subscriptionInDb != null)
        {
            if (subscriptionInDb.SubscriptionStatus == SubscriptionStatus.Active)
            {
                throw new RpcException(new Status(StatusCode.Internal, "Company Already Has Active Subscription"));
            }
            
            subscriptionInDb.SubscriptionPlanTypeId = request.SubscriptionPlanType;
            subscriptionInDb.SubscriptionStatus = SubscriptionStatus.Chosen;

            _db.Subscriptions.Update(subscriptionInDb);
        }
        else
        {
            var subscription = new Subscription
            {
                CustomerCompanyId = 13, //todo
                SubscriptionStatus = SubscriptionStatus.Chosen,
                SubscriptionPlanTypeId = request.SubscriptionPlanType
            };

            await _db.Subscriptions.AddAsync(subscription);
        }
        
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, "Could not insert subscription into db"));
        }
    }

    public async Task<GetSubscriptionDetailResponse> GetSubscriptionDetailsAsync(CallContext context = default)
    {
        var subscription = await _db.Subscriptions.SingleOrDefaultAsync(x => x.CustomerCompanyId == 13);

        if (subscription != null)
        {
            return new GetSubscriptionDetailResponse
            {
                SubscriptionPlanType = subscription.SubscriptionPlanTypeId,
                Price = _prices[subscription.SubscriptionPlanTypeId]
            };
        }
        else
        {
            throw new RpcException(new Status(StatusCode.Internal, "Subscription Not Chosen"));
        }
    }
}