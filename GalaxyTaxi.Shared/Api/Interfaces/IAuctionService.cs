using GalaxyTaxi.Shared.Api.Models.Auction;
using GalaxyTaxi.Shared.Api.Models.Filters;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace GalaxyTaxi.Shared.Api.Interfaces;

[Service("Auction")]
public interface IAuctionService
{
    Task Bid(BidRequest request, CallContext context = default);
    
    Task<GetAuctionsResponse> GetAuction(AuctionsFilter filter, CallContext context = default);
    
    Task<GetAuctionsCountResponse> GetAuctionCount(AuctionsFilter auctionsFilter);
    
    Task<GetSingleAuctionsResponse> GetSingleAuction(IdFilter idFilter);
    
    Task<GenerateAuctionsResponse> GenerateAuctionsForCompany(CallContext context = default);
    
    Task SaveEvaluation(SaveEvaluationRequest evaluation, CallContext context = default);
}