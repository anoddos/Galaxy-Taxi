namespace GalaxyTaxi.Api.Database.Models;

public class Bid
{
    public long Id { get; set; }

    public long AccountId { get; set; }

    public long AuctionId { get; set; }

    public double Amount { get; set; }

    public DateTime TimeStamp { get; set; }

    public Account Account { get; set; } = null!;

    public Auction Auction { get; set; } = null!;
}