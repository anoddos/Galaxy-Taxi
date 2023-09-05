using GalaxyTaxi.Shared.Api.Models.Auction;
using GalaxyTaxi.Shared.Api.Models.Common;
using GalaxyTaxi.Shared.Api.Models.Filters;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;

namespace GalaxyTaxi.Web.Pages.Auction;

public partial class AuctionsList
{
    private AuctionsFilter _auctionsFilter = new()
    {
        Status = ActionStatus.All,
        WonByMe = false,
        PageSize = 10,
        PageIndex = 0
    };

    private List<AuctionInfo> _auctionItems = new();
    private GoogleMap _map1;
    private MapOptions _mapOptions;

    private int _currentPage = 1;
    private int _totalPages;
    
    
    private AccountType loggedInAs;

    protected override void OnInitialized()
    {
        _mapOptions = new MapOptions
        {
            Zoom = 13,
            Center = new LatLngLiteral
            {
                Lat = 40.333,
                Lng = 41.33
            },
            MapTypeId = MapTypeId.Roadmap
        };
    }

    protected override async Task OnInitializedAsync()
    {
        loggedInAs = (await AccountService.GetAccountType()).AccountType;
        
        var totalCount = (await AuctionService.GetAuctionCount(_auctionsFilter)).TotalCount;
        var auctions = await AuctionService.GetAuction(_auctionsFilter);
        if (auctions != null)
        {
            _auctionItems = auctions.Auctions;
            _totalPages = (int)Math.Ceiling((double)totalCount / _auctionsFilter.PageSize);    
        }
    }

    private async Task UpdateGrid()
    {
        _auctionItems?.Clear();
        _auctionItems = (await AuctionService.GetAuction(_auctionsFilter))?.Auctions;
    }

    private async Task HandlePageClick(int page)
    {
        _auctionsFilter.PageIndex = page - 1;
        _currentPage = page;
        await UpdateGrid();
    }
    
    private void NavigateToAuctionDetails(long auctionId)
    {
        NavigationManager.NavigateTo($"/auction/{auctionId}");
    }
}