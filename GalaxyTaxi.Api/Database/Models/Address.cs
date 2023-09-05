namespace GalaxyTaxi.Api.Database.Models;

public class Address
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public double Latitude { get; set; }

    public double Longitude { get; set; }
    
    public bool IsDetected { get; set; }
}