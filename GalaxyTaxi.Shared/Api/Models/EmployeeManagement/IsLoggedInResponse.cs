using GalaxyTaxi.Shared.Api.Models.Common;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.EmployeeManagement;

[Serializable]
[ProtoContract]
public class IsLoggedInResponse
{
    [ProtoMember(1)]
    public bool IsLoggedIn { get; set; }
}
