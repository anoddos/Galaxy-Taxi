namespace GalaxyTaxi.Api.Database.Models;

public class Journey
{
    public long Id { get; set; }

    public long CustomerCompanyId { get; set; }
    
    public long VendorCompanyId { get; set; }
    
    public long OfficeId { get; set; }

    public Office Office { get; set; } = null!;

    public CustomerCompany CustomerCompany { get; set; } = null!;
    
    public VendorCompany VendorCompany { get; set; } = null!;

    public ICollection<Stop> Destinations { get; set; } = null!;
}