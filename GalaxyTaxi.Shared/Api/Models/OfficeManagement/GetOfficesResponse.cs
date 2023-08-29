using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.OfficeManagement;

[Serializable]
[ProtoContract]
public class GetOfficesResponse
{
    [ProtoMember(1)]
    public List<OfficeInfo> Offices { get; set; }
}