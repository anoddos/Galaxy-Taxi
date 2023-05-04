using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Login;

[Serializable]
[ProtoContract]
public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}