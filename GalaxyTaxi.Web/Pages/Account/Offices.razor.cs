using GalaxyTaxi.Shared.Api.Models.Filters;
using GalaxyTaxi.Shared.Api.Models.OfficeManagement;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents;
using GalaxyTaxi.Shared.Api.Models.AddressDetection;

namespace GalaxyTaxi.Web.Pages.Account;

public partial class Offices
{
    private List<OfficeInfo> _offices = new List<OfficeInfo>();
    private bool _loaded;
    public OfficeInfo OfficeFilter { get; set; } = new OfficeInfo { Address = new AddressInfo(), OfficeId = -1 };
    private string navigationLink { get; set; } = "";
    private GoogleMap map1;

    private MapOptions mapOptions { get; set; } = new MapOptions
    {
        Zoom = 13,
        Center = new LatLngLiteral { Lat = 41.716667, Lng = 44.783333 },
        MapTypeId = MapTypeId.Roadmap
    };

    private Marker marker;

    protected override async Task OnInitializedAsync()
    {
        await ReloadOffices();
        _loaded = true;
    }

    private async Task ReloadOffices()
    {
        var response = await _officeManagement.GetOffices(new OfficeManagementFilter());
        if (response != null && response.Offices != null)
        {
            _offices = response.Offices;
        }

        navigationLink = $"/account/employeesInfo";
    }

    private async Task AddMarker()
    {
        OfficeFilter.Address = await _addressDetection.DetectAddressCoordinatesFromName(OfficeFilter.Address);

        var position = new LatLngLiteral
            { Lat = (double)OfficeFilter.Address.Latitude, Lng = (double)OfficeFilter.Address.Longitude };


        if (marker == null)
        {
            marker = await Marker.CreateAsync(map1.JsRuntime, new MarkerOptions()
            {
                Map = map1.InteropObject,
                Draggable = true
            });
        }

        await marker.SetPosition(position);
        StateHasChanged();

        await marker.AddListener("dragend", async () =>
        {
            var position = await marker.GetPosition();
            OfficeFilter.Address.Longitude = position.Lng;
            OfficeFilter.Address.Latitude = position.Lat;
            var detectedAddress = await _addressDetection.DetectAddressNameFromCoordinates(OfficeFilter.Address);
            OfficeFilter.Address.Name = detectedAddress.Name;
            StateHasChanged();
        });
    }

    private async Task SaveChanges()
    {
        if (!string.IsNullOrWhiteSpace(OfficeFilter.Address.Name))
        {
            OfficeFilter.Address = await _addressDetection.DetectAddressCoordinatesFromName(OfficeFilter.Address);
        }

        if (OfficeFilter.OfficeId == -1)
        {
            await _officeManagement.AddOffice(OfficeFilter);
            await ReloadOffices();
            StateHasChanged();
        }
        else
        {
            await _officeManagement.EditOfficeDetails(OfficeFilter);
        }
    }

    private async Task OfficeValueChanged(OfficeInfo current)
    {
        if (current == null)
        {
            OfficeFilter = new OfficeInfo { Address = new AddressInfo(), OfficeId = -1 };
        }
        else
        {
            OfficeFilter = current;
            mapOptions.Center = new LatLngLiteral
                { Lat = (double)OfficeFilter.Address.Latitude, Lng = (double)OfficeFilter.Address.Longitude };
        }

        StateHasChanged();
    }
}