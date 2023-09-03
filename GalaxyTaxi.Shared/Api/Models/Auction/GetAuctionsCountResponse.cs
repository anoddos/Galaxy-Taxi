using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Auction;

[ProtoContract]
[Serializable]
public class GetAuctionsCountResponse
{
    [ProtoMember(1)] 
    public int TotalCount { get; set; }
}