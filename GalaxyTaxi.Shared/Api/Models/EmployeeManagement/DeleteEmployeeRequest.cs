using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.EmployeeManagement;

[ProtoContract]
[Serializable]
public class DeleteEmployeeRequest
{
    [ProtoMember(1)]
    public long EmployeeId { get; set; }
    
}