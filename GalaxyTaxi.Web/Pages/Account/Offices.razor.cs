using GalaxyTaxi.Shared.Api.Models.Filters;
using GalaxyTaxi.Shared.Api.Models.OfficeManagement;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents;
using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using MudBlazor;
using GalaxyTaxi.Shared.Api.Models.AccountSettings;
using Grpc.Core;

namespace GalaxyTaxi.Web.Pages.Account;

public partial class Offices
{
    private List<OfficeInfo> _offices = new List<OfficeInfo>();
    private bool _loaded;
    public OfficeInfo OfficeFilter { get; set; } = new OfficeInfo { Address = new AddressInfo(), OfficeId = -1 };
    private string navigationLink { get; set; } = "";
    private GoogleMap map1;
    private string DetectedStatus = "";

	private bool showAlert = false;
	private string alertMessage = "";
	private Severity alertSeverity;

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
        if (_offices != null && _offices.Any())
		{
			OfficeFilter = _offices.First();
		}
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

    private async Task OnFromAddressChanged(string newValue)
    {
        OfficeFilter.Address.Name = newValue;
        await AddMarker();
    }

    private async Task AddMarker()
    {
		OfficeFilter.Address = string.IsNullOrWhiteSpace(OfficeFilter.Address.Name) ? new AddressInfo()
																	: await _addressDetection.DetectAddressCoordinatesFromName(OfficeFilter.Address);



        var position = OfficeFilter.Address.IsDetected ? new LatLngLiteral{ Lat = (double)OfficeFilter.Address.Latitude, Lng = (double)OfficeFilter.Address.Longitude }
			                                           : new LatLngLiteral { Lat = 41.716667, Lng = 44.783333 };

        if (!OfficeFilter.Address.IsDetected) {
            OfficeFilter.Address.Name = "";
        }


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


	private void AddNewOffice()
	{
		OfficeFilter = new OfficeInfo { Address = new AddressInfo(), OfficeId = -1 };
	}
	private async Task SaveChanges()
    {

		StateHasChanged();

		try
		{
			if (OfficeFilter.OfficeId == -1)
			{
				OfficeFilter = await _officeManagement.AddOffice(OfficeFilter);
				alertMessage = "Office Details Saved";

				await ReloadOffices();
			}
			else
			{
				OfficeFilter = await _officeManagement.EditOfficeDetails(OfficeFilter);
				alertMessage = "Office Details Updated";
			}
			alertSeverity = Severity.Success;
		}
		catch (RpcException ex)
		{
			alertMessage = ex.Status.Detail;
			alertSeverity = Severity.Error;
		}

		showAlert = true;
		StateHasChanged();
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
            DetectedStatus = current.Address.IsDetected ? "" : "[Not Detected]";
            mapOptions.Center = new LatLngLiteral
                { Lat = (double)OfficeFilter.Address.Latitude, Lng = (double)OfficeFilter.Address.Longitude };
        }

        StateHasChanged();
    }

	private void HandleAlertClosed()
	{
		showAlert = false;
		StateHasChanged();
	}
}