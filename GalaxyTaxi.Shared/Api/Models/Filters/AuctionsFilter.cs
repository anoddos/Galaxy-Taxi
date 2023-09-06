using System.ComponentModel;
using GalaxyTaxi.Shared.Api.Models.Common;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Filters;

[ProtoContract]
[Serializable]
public class AuctionsFilter
{
    [ProtoMember(1)]
    public ActionStatus Status { get; set; }
    
    [ProtoMember(2)]
    public bool WonByMe { get; set; }
    
    [ProtoMember(3)]
    public int PageSize { get; set; }
    
    [ProtoMember(4)]
    public int PageIndex { get; set; }
    
    [ProtoMember(5)]
    public bool ToBeEvaluated { get; set; }
}