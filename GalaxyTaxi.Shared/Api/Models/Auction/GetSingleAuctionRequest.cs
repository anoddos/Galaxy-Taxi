using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Auction;

[ProtoContract]
[Serializable]
public class GetSingleAuctionRequest
{
	[ProtoMember(1)]
	public long AuctionId { get; set;}
}
