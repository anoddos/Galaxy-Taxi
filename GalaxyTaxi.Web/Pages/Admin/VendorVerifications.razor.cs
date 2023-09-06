using GalaxyTaxi.Shared.Api.Models.Common;
using GalaxyTaxi.Shared.Api.Models.Filters;
using GalaxyTaxi.Shared.Api.Models.VendorCompany;
using MudBlazor;

namespace GalaxyTaxi.Web.Pages.Admin;

public partial class VendorVerifications
{
    private List<VendorInfo> _vendors = new();
    
    private bool showAlert = false;
    private string alertMessage = "";
    private Severity alertSeverity;
    private ProblematicAuctionsFilter filter = new();
    private VendorInfo selectedVendor;
    private IDialogReference dialogReference;
    
    private VendorFilter _vendorFilter = new()
    {
        Status = AccountStatus.Pending
    };
    
    
    protected override async Task OnInitializedAsync()
    {
        List<VendorInfo> response = (await AccountService.GetVendorCompanies(_vendorFilter)).Vendors;
        
        if (response != null)
        {
            _vendors = response;
        }
        
        StateHasChanged();
    }

    private void HandleAlertClosed()
    {
        showAlert = false;
        StateHasChanged();
    }

    private async Task Search()
    {
        var response = (await AccountService.GetVendorCompanies(_vendorFilter)).Vendors;
        
        if (response != null)
        {
            _vendors = response;
        }
        StateHasChanged();
    }
    
    private async void OpenEditPopup(VendorInfo vendor)
    {
        selectedVendor = vendor;
        var parameters = new DialogParameters { ["Vendor"] = selectedVendor};
        dialogReference =  await DialogService.ShowAsync<VendorPopup>("Edit Vendor Status", parameters);
        var result = await dialogReference.Result;
        StateHasChanged();
    }
}