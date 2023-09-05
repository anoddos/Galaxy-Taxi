using GalaxyTaxi.Shared.Api.Models.Admin;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Net.NetworkInformation;

namespace GalaxyTaxi.Web.Pages.Admin
{

	public partial class VendorPopup
	{
		[Parameter]
		public VendorInfo Vendor { get; set; } = new VendorInfo();



	}
}
