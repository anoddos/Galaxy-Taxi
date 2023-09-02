using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.AccountSettings;

[ProtoContract]
[Serializable]
public class PasswordValidationResponse
{
    [ProtoMember(1)]
    public bool IsValid { get; set; }
}

