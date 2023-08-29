using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.EmployeeManagement;

[Serializable]
[ProtoContract]
public class GetEmployeesResponse
{
    [ProtoMember(1)]
    public List<EmployeeJourneyInfo> Employees { get; set; } = null!;
}