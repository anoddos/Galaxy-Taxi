using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Auction;

[ProtoContract]
[Serializable]
public class GetAuctionsRequest
{
    [ProtoMember(1)]
    public bool IsFinished { get; set; }

    [ProtoMember(2)]
    public bool IncludesMe { get; set; }
    
    [ProtoMember(3)]
    public bool WonByMe { get; set; }
}