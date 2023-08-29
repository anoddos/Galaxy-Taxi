using GalaxyTaxi.Shared.Api.Models.Common;

namespace GalaxyTaxi.Api.Database.Models;

public class Payment
{
    public long Id { get; set; }

    public long? AuctionId { get; set; }

    public long? SubscriptionId { get; set; }

    public Auction? Auction { get; set; } = null!;

    public Subscription? Subscription { get; set; } = null!;

    public PaymentStatus PaymentStatusId { get; set; }

    public PaymentType PaymentTypeId { get; set; }
}