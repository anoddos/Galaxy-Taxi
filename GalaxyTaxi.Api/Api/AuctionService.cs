using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Api.Database.Models;
using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.Auction;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Api.Api;

public class AuctionService : IAuctionService
{
    
    private readonly Db _db;
    
    public AuctionService(Db db)
    {
        _db = db;
    }

    public async Task Bid(BidRequest request, CallContext context = default)
    {
        await ValidateBidRequestAsync(request);

        var bid = new Bid
        {
            Amount = request.Amount,
            AccountId = request.AccountId,
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
    
    private async Task ValidateBidRequestAsync(BidRequest request, CallContext context = default)
    {
        var lastBid =  (await _db.Bids.LastAsync(x => x.AuctionId == request.AuctionId)).Amount;
        if (lastBid <= request.Amount)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "New Bid Should Have Smaller Value"));
        }
    }
}