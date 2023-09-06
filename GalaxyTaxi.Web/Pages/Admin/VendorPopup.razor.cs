using GalaxyTaxi.Shared.Api.Models.VendorCompany;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Net.NetworkInformation;
using GalaxyTaxi.Shared.Api.Models.VendorCompany;
using GalaxyTaxi.Shared.Api.Models.AccountSettings;
using GalaxyTaxi.Shared.Api.Models.Common;
using GalaxyTaxi.Shared.Api.Models.Admin;
using MudBlazor;
using Grpc.Core;
using static MudBlazor.CategoryTypes;

namespace GalaxyTaxi.Web.Pages.Admin
{

	public partial class VendorPopup
	{
		[Parameter]
		public VendorInfo Vendor { get; set; } = new VendorInfo();

		[CascadingParameter]
		MudDialogInstance dialog { get; set; }


		private bool showAlert = false;
		private string alertMessage = "";
		private Severity alertSeverity;

		IList<VendorFileModel> files = new List<VendorFileModel>();

		protected override async Task OnInitializedAsync()
		{
			var response = await AccountService.GetVendorFiles(new GetVendorFilesRequest{ VendorId = Vendor.VendorId});
			if (response != null)
			{
				files = response.VendorFiles;
			}
		}

		private async Task UpdateVendor(AccountStatus newStatus)
		{
			try
			{
				await AccountService.RespondToVendor(new RespondToVendorRequest { VendorEmail = Vendor.Email, NewStatus = newStatus });
				alertMessage = "Vendor Status Updated";
				alertSeverity = Severity.Success;
			}
			catch(RpcException ex)
			{
				alertMessage = ex.Status.Detail;
				alertSeverity = Severity.Error;
			}
		}

		private void CloseDialog()
		{
			dialog.Close(DialogResult.Ok(true));
		}

		private void HandleAlertClosed()
		{
			showAlert = false;
			StateHasChanged();
		}

	}
}
