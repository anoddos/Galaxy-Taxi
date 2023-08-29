using GalaxyTaxi.Shared.Api.Models.Filters;
using GalaxyTaxi.Shared.Api.Models.OfficeManagement;

namespace GalaxyTaxi.Web.Pages.Account;

public partial class Offices
{
    private List<OfficeInfo> _offices = new();
    private bool _loaded;

    protected override async Task OnInitializedAsync()
    {
        var response = await _officeManagement.GetOffices(new OfficeManagementFilter());
        if (response != null)
        {
            _offices = response.Offices;
        }

        _loaded = true;
    }
}