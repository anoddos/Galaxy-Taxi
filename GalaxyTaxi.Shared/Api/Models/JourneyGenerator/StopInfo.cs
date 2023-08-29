using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.JourneyGenerator;
[ProtoContract]
[Serializable]
public class StopInfo
{
    [ProtoMember(1)]
    public long Id { get; set; }
    
    [ProtoMember(2)]
    public AddressInfo Address { get; set; } = null!;
    
    [ProtoMember(3)]
    public SingleEmployeeInfo EmployeeDetails { get; set; } = null!;
}