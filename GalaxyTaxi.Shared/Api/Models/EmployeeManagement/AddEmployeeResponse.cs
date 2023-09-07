using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
[ProtoContract]
[Serializable]
public class AddEmployeeResponse
{
	[ProtoMember(1)]
	public int SuccessfulCount { get; set; }

	[ProtoMember(2)]
	public int FailedCount { get; set; }
}
