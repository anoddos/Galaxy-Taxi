namespace GalaxyTaxi.Api.Database.Models
{
	public class VendorFile
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public string Path { get; set; }
		public DateTime UploadDate { get; set; }
		public long VendorCompanyId { get; set; }
		public VendorCompany VendorCompany { get; set; } = null!;
	}
}
