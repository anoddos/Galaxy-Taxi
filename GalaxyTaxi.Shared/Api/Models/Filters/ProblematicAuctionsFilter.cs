using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Filters;

[Serializable]
[ProtoContract]
public class ProblematicAuctionsFilter
{
    [ProtoMember(1)]
    public bool Resolved { get; set; }
}