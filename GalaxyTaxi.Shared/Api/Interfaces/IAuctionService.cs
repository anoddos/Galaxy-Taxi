using GalaxyTaxi.Shared.Api.Models.Auction;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace GalaxyTaxi.Shared.Api.Interfaces;

[Service("Auction")]
public interface IAuctionService
{
    Task Bid(BidRequest request, CallContext context = default);
    
    Task<GetAuctionsResponse> GetAuction(GetAuctionsRequest request, CallContext context = default);
}