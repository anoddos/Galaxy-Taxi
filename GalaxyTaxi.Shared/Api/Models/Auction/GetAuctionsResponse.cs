using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Auction;

[ProtoContract]
[Serializable]
public class GetAuctionsResponse
{
    [ProtoMember(1)]
    public List<AuctionInfo> Auctions { get; set; }
}