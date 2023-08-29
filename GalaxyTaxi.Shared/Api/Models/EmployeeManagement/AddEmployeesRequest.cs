using ProtoBuf;
using System.Collections.Generic;

namespace GalaxyTaxi.Shared.Api.Models.EmployeeManagement;

[Serializable]
[ProtoContract]
public class AddEmployeesRequest
{
    [ProtoMember(1)]
    public List<SingleEmployeeInfo> EmployeesInfo { get; set; }  = null!;
}