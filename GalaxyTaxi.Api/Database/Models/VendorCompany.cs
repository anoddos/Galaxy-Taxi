namespace GalaxyTaxi.Api.Database.Models;

public class VendorCompany
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public long AccountId { get; set; }

    public string IdentificationCode { get; set; } = null!;

    public Account Account { get; set; } = null!;
    
    public List<VendorFile> VendorFiles { get; set; } = new List<VendorFile>();
}