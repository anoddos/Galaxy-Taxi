using GalaxyTaxi.Shared.Api.Models.Auction;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Shared.Api.Interfaces;

public interface IAuctionService
{
    Task Bid(BidRequest request, CallContext context = default);
}