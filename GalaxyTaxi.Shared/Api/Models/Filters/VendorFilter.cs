using GalaxyTaxi.Shared.Api.Models.Common;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Filters;

[Serializable]
[ProtoContract]
public class VendorFilter
{
    [ProtoMember(1)]
    public AccountStatus Status { get; set; }
}