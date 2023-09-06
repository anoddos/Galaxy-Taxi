using GalaxyTaxi.Shared.Api.Models.Common;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.Admin;

[ProtoContract]
[Serializable]
public class RespondToVendorRequest
{
	[ProtoMember(1)]
	public string VendorEmail { get; set; }

	[ProtoMember(2)]
	public AccountStatus NewStatus { get; set; }
}
