using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.EmployeeManagement;

[Serializable]
[ProtoContract]
public class GetEmployeesResponse
{
    [ProtoMember(1)]
    public List<EmployeeJourneyInfo> Employees { get; set; } = null!;
}