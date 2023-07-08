using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.Auction;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Api.Api;

public class AuctionService : IAuctionService
{
    public Task Bid(BidRequest request, CallContext context = default)
    {
        throw new NotImplementedException();
    }
}