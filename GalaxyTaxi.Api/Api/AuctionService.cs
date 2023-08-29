using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Api.Database.Models;
using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.Auction;
using GalaxyTaxi.Shared.Api.Models.Common;
using GalaxyTaxi.Shared.Api.Models.Filters;
using GalaxyTaxi.Shared.Api.Models.JourneyGenerator;
using GalaxyTaxi.Shared.Api.Models.Register;
using GalaxyTaxi.Shared.Api.Models.VendorCompany;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Api.Api;

public class AuctionService : IAuctionService
{
    private readonly Db _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuctionService(Db db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task Bid(BidRequest request, CallContext context = default)
    {
        await ValidateBidRequestAsync(request);

        var bid = new Bid
        {
            Amount = request.Amount,
            AccountId = GetAccountId(),
            AuctionId = request.AuctionId,
            TimeStamp = DateTime.UtcNow
        };

        try
        {
            await _db.Bids.AddAsync(bid);

            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, "Could not insert into db"));
        }
    }

    public async Task<GetAuctionsResponse> GetAuction(AuctionsFilter filter, CallContext context = default)
    {
        var accountId = GetAccountId();

        var auctions = _db.Auctions.Include(x => x.Bids).Where(x => (filter.IsFinished == false || x.CurrentWinner != null) &&
                                                            (filter.WonByMe == false || x.CurrentWinnerId == accountId) &&
                                                            (filter.IncludesMe == false || x.Journey.CustomerCompany.AccountId == accountId || x.Bids.Any(xx => xx.AccountId == accountId)))
            .Select(x => new AuctionInfo
            {
                Id = x.Id,
                Amount = x.Amount,
                StartTime = x.StartTime,
                EndTime = x.EndTime,
                Bids = x.Bids.Select(xx => new BidInfo
                {
                    Id = xx.Id,
                    TimeStamp = xx.TimeStamp,
                    Amount = xx.Amount,
                    Account = new AccountInfo
                    {
                        CompanyName = xx.Account.CompanyName,
                        Id = xx.Account.Id
                    }
                }).ToList(),
                CurrentWinner = x.CurrentWinnerId == null ? null : new VendorCompanyInfo
                {
                    Id = (long)x.CurrentWinnerId,
                    Name = x.CurrentWinner.Name
                },
                JourneyInfo = new JourneyInfo
                {

                }
            });

        return new GetAuctionsResponse { Auctions = await auctions.ToListAsync() };
    }

    private async Task ValidateBidRequestAsync(BidRequest request, CallContext context = default)
    {
        var lastBid = (await _db.Bids.LastAsync(x => x.AuctionId == request.AuctionId)).Amount;
        if (lastBid <= request.Amount)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "New Bid Should Have Smaller Value"));
        }
    }

    private string GetSessionValue(string key, CallContext context = default)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var res = httpContext?.User.Claims.FirstOrDefault(c => c.Type == key);

        return res == null ? "" : res.Value;
    }

    private long GetAccountId()
    {
        var accountId = GetSessionValue(AuthenticationKey.AccountId);

        if (string.IsNullOrWhiteSpace(accountId))
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Not Logged In"));
        }

        return long.Parse(accountId);
    }
}