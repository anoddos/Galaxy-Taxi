using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Auction;

[ProtoContract]
[Serializable]
public class GetAuctionsResponse
{
    [ProtoMember(1)]
    public long AuctionId { get; set; }

    [ProtoMember(2)]
    public double Amount { get; set; }
}