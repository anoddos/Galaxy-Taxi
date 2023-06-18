namespace GalaxyTaxi.Api.Database.Models;

public class VendorCompany
{
    public long Id { get; set; }
    
    public string Name { get; set; } = null!;

    public string IdentificationCode { get; set; } = null!;
}