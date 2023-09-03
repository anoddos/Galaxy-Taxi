using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Auction;

[ProtoContract]
[Serializable]
public class GetSingleAuctionsResponse
{
    [ProtoMember(1)] 
    public AuctionInfo Auction { get; set; } = null!;
}