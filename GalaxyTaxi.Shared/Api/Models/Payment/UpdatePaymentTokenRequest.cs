using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.Payment;

[ProtoContract]
[Serializable]
public class UpdatePaymentTokenRequest
{
	[ProtoMember(1)]
	public string Token { get; set; }
}
