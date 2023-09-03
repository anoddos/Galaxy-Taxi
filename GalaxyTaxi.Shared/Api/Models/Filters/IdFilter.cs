using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Filters;

[Serializable]
[ProtoContract]
public class IdFilter
{
    [ProtoMember(1)]
    public long Id { get; set; }
}