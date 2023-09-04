using GalaxyTaxi.Shared.Api.Models.CustomerCompany;
using GalaxyTaxi.Shared.Api.Models.OfficeManagement;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.JourneyGenerator;

[ProtoContract]
[Serializable]
public class JourneyInfo
{
    [ProtoMember(1)]
    public long Id { get; set; }

    [ProtoMember(2)]
    public OfficeInfo Office { get; set; } = null!;

    [ProtoMember(3)]
    public CustomerCompanyInfo CustomerCompany { get; set; } = null!;
    
    [ProtoMember(4)]
    public IEnumerable<StopInfo> Stops { get; set; } = null!;
}