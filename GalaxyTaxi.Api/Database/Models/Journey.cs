namespace GalaxyTaxi.Api.Database.Models;

public class Journey
{
    public long Id { get; set; }

    public long CustomerId { get; set; }
    
    public long OfficeId { get; set; }

    public Office Office { get; set; } = null!;

    public Account Customer { get; set; } = null!;

    public ICollection<Destination> Destinations { get; set; } = null!;
}