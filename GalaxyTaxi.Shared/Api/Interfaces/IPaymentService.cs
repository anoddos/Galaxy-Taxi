using GalaxyTaxi.Shared.Api.Models.Payment;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Shared.Api.Interfaces;

public interface IPaymentService
{
    Task<PaymentResponse> PayForRides(PayForRidesRequest request, CallContext context = default);
    Task<PaymentResponse> PayForSubscription(PayForSubscriptionRequest request, CallContext context = default);
}