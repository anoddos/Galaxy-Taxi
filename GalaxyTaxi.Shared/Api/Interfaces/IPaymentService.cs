using GalaxyTaxi.Shared.Api.Models.Payment;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace GalaxyTaxi.Shared.Api.Interfaces;

[Service("Payment")]
public interface IPaymentService
{
    Task<PaymentResponse> PayForSubscription(PayForSubscriptionRequest request, CallContext context = default);
}