namespace GalaxyTaxi.Api.Database.Models;

public class Auction
{
    public long Id { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public long JourneyId { get; set; }

    public decimal Amount { get; set; }

    public long? CurrentWinnerId { get; set; }
    
    public long CustomerCompanyId { get; set; }

    public VendorCompany? CurrentWinner { get; set; }
    
    public CustomerCompany CustomerCompany { get; set; } = null!;

    public Journey Journey { get; set; } = null!;

    public ICollection<Bid> Bids { get; set; } = null!;
}