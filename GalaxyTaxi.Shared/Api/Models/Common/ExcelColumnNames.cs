namespace GalaxyTaxi.Shared.Api.Models.Common
{
	public static class ExcelColumnNames
	{
		public const string FirstName = "Firs tName";
		public const string LastName = "Last Name";
		public const string Mobile = "Mobile";
		public const string OfficeId = "Office Id";
		public const string Address = "Address";

		public static readonly List<string> AllColumns = new List<string> { FirstName, LastName, Mobile, OfficeId, Address };
	}
}
