using GalaxyTaxi.Shared.Api.Models.Payment;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace GalaxyTaxi.Shared.Api.Interfaces;

[Service("Payment")]
public interface IPaymentService
{
    Task<PaymentResponse> PayForRides(PayForRidesRequest request, CallContext context = default);
    Task<PaymentResponse> PayForSubscription(PayForSubscriptionRequest request, CallContext context = default);
}