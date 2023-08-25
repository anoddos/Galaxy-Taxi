using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.EmployeeManagement;

[ProtoContract]
public class SingleEmployeeInfo
{
    [ProtoMember(1)]
    public string FirstName { get; set; } = null!;
    
    [ProtoMember(2)]
    public string LastName { get; set; } = null!;
    
    [ProtoMember(3)]
    public string Mobile { get; set; } = null!;
    
    [ProtoMember(4)]
    public long CustomerCompanyId { get; set; }
    
    [ProtoMember(5)]
    public long OfficeId { get; set; }
    
    [ProtoMember(6)]
    public string Address { get; set; } = null!;
}