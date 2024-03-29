using System.ComponentModel.DataAnnotations.Schema;
using GalaxyTaxi.Shared.Api.Models.Common;

namespace GalaxyTaxi.Api.Database.Models;

public class Auction
{
    public long Id { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }
    
    [Column(TypeName = "Date")]
    public DateTime FromDate { get; set; }
    
    [Column(TypeName = "Date")]
    public DateTime ToDate { get; set; }

    public long JourneyId { get; set; }

    public double Amount { get; set; }
    
    public double? FulfillmentPercentage { get; set; }

    public long? CurrentWinnerId { get; set; }
    
    public bool PaymentProcessed { get; set; }
    
    public long CustomerCompanyId { get; set; }
    
    public Feedback? FeedbackId { get; set; }

    public string? Comment { get; set; } = null!;

    public VendorCompany? CurrentWinner { get; set; }
    
    public CustomerCompany CustomerCompany { get; set; } = null!;

    public Journey Journey { get; set; } = null!;

    public ICollection<Bid> Bids { get; set; } = null!;
}