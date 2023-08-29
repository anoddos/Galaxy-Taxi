using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Login;

[Serializable]
[ProtoContract]
public class LoginRequest
{
    [ProtoMember(1)]
    public string Email { get; set; } = null!;

    [ProtoMember(2)]
    public string Password { get; set; } = null!;
}