using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using GalaxyTaxi.Shared.Api.Models.OfficeManagement;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.EmployeeManagement;

[Serializable]
[ProtoContract]
public class EmployeeJourneyInfo
{
    [ProtoMember(1)]
    public string FirstName { get; set; }
    
    [ProtoMember(2)]
    public string LastName { get; set; }
    
    [ProtoMember(3)]
    public AddressInfo From { get; set; }
    
    [ProtoMember(4)]
    public OfficeInfo To { get; set; }
    
    [ProtoMember(5)]
    public string Mobile { get; set; }
    [ProtoMember(6)]
    public long EmployeeId { get; set; }
}