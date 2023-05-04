namespace GalaxyTaxi.Api.Database.Models;

public class Address
{
    public long Id { get; set; }

    public string Name { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }
}