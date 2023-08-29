using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.Payment;
using ProtoBuf.Grpc;
using Stripe;

namespace GalaxyTaxi.Api.Api;

public class PaymentService : IPaymentService
{
    private const string StripeSecretKey = "your_stripe_secret_key";

    public Task<PaymentResponse> ProcessPayment(PaymentRequest request, CallContext context)
    {
        StripeConfiguration.ApiKey = StripeSecretKey;

        // Use Stripe SDK to create a payment intent
        var options = new PaymentIntentCreateOptions
        {
            Amount = request.Amount,
            Currency = "gel",
            PaymentMethodTypes = new List<string> { "card" }
        };
        var service = new PaymentIntentService();
        var paymentIntent = service.Create(options);

        // Return payment intent information to the client
        return Task.FromResult(new PaymentResponse
        {
            PaymentIntentId = paymentIntent.Id
        });
    }
}