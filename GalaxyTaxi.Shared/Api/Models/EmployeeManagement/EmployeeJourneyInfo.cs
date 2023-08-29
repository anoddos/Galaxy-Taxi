using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using GalaxyTaxi.Shared.Api.Models.OfficeManagement;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.EmployeeManagement;

[Serializable]
[ProtoContract]
public class EmployeeJourneyInfo
{
    [ProtoMember(1)]
    public string FirstName { get; set; } = null!;
    
    [ProtoMember(2)]
    public string LastName { get; set; } = null!;
    
    [ProtoMember(3)]
    public AddressInfo From { get; set; } = null!;
    
    [ProtoMember(4)]
    public OfficeInfo To { get; set; } = null!;
    
    [ProtoMember(5)]
    public string Mobile { get; set; } = null!;
    
    [ProtoMember(6)]
    public long EmployeeId { get; set; }
}