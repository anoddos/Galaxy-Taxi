using GalaxyTaxi.Shared.Api.Models.OfficeManagement;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Filters;

[Serializable]
[ProtoContract]
public class EmployeeManagementFilter
{
    [ProtoMember(1)]
    public OfficeInfo? SelectedOffice { get; set; }

    [ProtoMember(2)]
    public string EmployeeName { get; set; } = "";
}