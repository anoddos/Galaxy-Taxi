namespace GalaxyTaxi.Api.Database.Models;

public class Address
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }
    
    public bool IsDetected { get; set; }
}