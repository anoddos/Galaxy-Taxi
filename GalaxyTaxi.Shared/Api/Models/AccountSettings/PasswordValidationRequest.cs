using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.AccountSettings;
[ProtoContract]
[Serializable]
public class PasswordValidationRequest
{
    [ProtoMember(1)]
    public string OldPassword { get; set; }
    [ProtoMember(2)]
    public string NewPassword { get; set; }
}
