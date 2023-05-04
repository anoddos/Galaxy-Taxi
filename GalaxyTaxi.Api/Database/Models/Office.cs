namespace GalaxyTaxi.Api.Database.Models;

public class Office
{
    public long Id { get; set; }
    
    public long AddressId { get; set; }

    public Address Address { get; set; } = null!;

    public DateTime WorkingStartTime { get; set; }
    
    public DateTime WorkingEndTime { get; set; }
}