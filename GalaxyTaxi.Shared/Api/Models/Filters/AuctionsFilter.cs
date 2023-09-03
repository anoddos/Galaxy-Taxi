using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Filters;

[ProtoContract]
[Serializable]
public class AuctionsFilter
{
    [ProtoMember(1)]
    public bool IsFinished { get; set; }

    [ProtoMember(2)]
    public bool IncludesMe { get; set; }

    [ProtoMember(3)]
    public bool WonByMe { get; set; }
    
    [ProtoMember(4)]
    public int PageSize { get; set; }
    
    [ProtoMember(5)]
    public int PageIndex { get; set; }

    [ProtoMember(6)]
    public long AuctionId { get; set; } = -1;
}