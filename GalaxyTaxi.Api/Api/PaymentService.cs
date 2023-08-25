using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.Payment;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Api.Api;

public class PaymentService : IPaymentService
{
    public Task<PaymentResponse> PayForSubscription(PayForSubscriptionRequest request, CallContext context = default)
    {
        throw new NotImplementedException();
    }
}

