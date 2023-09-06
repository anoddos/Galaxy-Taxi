using GalaxyTaxi.Shared.Api.Models.VendorCompany;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Net.NetworkInformation;
using GalaxyTaxi.Shared.Api.Models.VendorCompany;
using GalaxyTaxi.Shared.Api.Models.AccountSettings;

namespace GalaxyTaxi.Web.Pages.Admin
{

	public partial class VendorPopup
	{
		[Parameter]
		public VendorInfo Vendor { get; set; } = new VendorInfo();

		IList<VendorFileModel> files = new List<VendorFileModel>();

		protected override async Task OnInitializedAsync()
		{
			var response = await AccountService.GetVendorFiles(new GetVendorFilesRequest{ VendorId = Vendor.VendorId});
			if (response != null)
			{
				files = response.VendorFiles;
			}
		}

	}
}
