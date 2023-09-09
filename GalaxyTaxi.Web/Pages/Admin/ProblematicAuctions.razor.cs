using GalaxyTaxi.Shared.Api.Models.Auction;
using GalaxyTaxi.Shared.Api.Models.Filters;
using MudBlazor;

namespace GalaxyTaxi.Web.Pages.Admin;

public partial class ProblematicAuctions
{
    private List<AuctionInfo> _auctions = new();
    
    private bool showAlert = false;
    private string alertMessage = "";
    private Severity alertSeverity;
    private ProblematicAuctionsFilter filter = new();
    
    protected override async Task OnInitializedAsync()
    {
        var response = (await AuctionService.GetProblematicAuctions(new ProblematicAuctionsFilter
        {
            Resolved = false
        })).Auctions;
        
        if (response != null)
        {
            _auctions = response;
        }
        StateHasChanged();
    }

    private async Task SaveData(AuctionInfo contextItem)
    {
        await AuctionService.UpdateFulfilmentPercentage(new UpdateFulfilmentPercentageRequest
        {
            Id = contextItem.Id,
            Percentage = (double)contextItem.Percentage!
        });
        
        alertMessage = "Fulfillment percentage was updated successfully.";
        alertSeverity = Severity.Success;
        showAlert = true;
        StateHasChanged();
    }
    
    private void HandleAlertClosed()
    {
        showAlert = false;
        StateHasChanged();
    }

    private async Task Search()
    {
        var response = (await AuctionService.GetProblematicAuctions(filter)).Auctions;
        
        if (response != null)
        {
            _auctions = response;
        }
        else
        {
            _auctions = new List<AuctionInfo>();
        }
        StateHasChanged();
    }
}