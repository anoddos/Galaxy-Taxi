using GalaxyTaxi.Shared.Api.Models.Common;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Register;

[ProtoContract]
[Serializable]
public class RegisterRequest
{
    [ProtoMember(1)]
    public string CompanyName { get; set; } = null!;

    [ProtoMember(2)]
    public string CompanyEmail { get; set; } = null!;

    [ProtoMember(3)]
    public string Password { get; set; } = null!;

    [ProtoMember(4)] 
    public AccountType Type { get; set; }
}
