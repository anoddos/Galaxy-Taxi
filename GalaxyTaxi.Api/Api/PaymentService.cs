using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.Common;
using GalaxyTaxi.Shared.Api.Models.Payment;
using ProtoBuf.Grpc;
using Stripe;

namespace GalaxyTaxi.Api.Api;

public class PaymentService : IPaymentService
{

	public async Task<PaymentResponse> ProcessPayment(PaymentRequest request, CallContext context)
	{
		StripeConfiguration.ApiKey = PaymentInfo.StripeSecretKey;

		// Use Stripe SDK to create a payment intent
		StripeConfiguration.ApiKey = PaymentInfo.StripeSecretKey;


		var chargeOptions = new ChargeCreateOptions
		{
			Amount = request.Amount, // e.g. $10.00, adjust accordingly
			Currency = "GEL",
			Description = "Description for the charge",
			Source = request.token
		};

		var chargeService = new ChargeService();

		var charge = await chargeService.CreateAsync(chargeOptions);
		return new PaymentResponse { PaymentStatus = charge.Status };
	}
}