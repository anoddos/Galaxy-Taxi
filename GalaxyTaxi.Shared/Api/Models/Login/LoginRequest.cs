using System.ComponentModel.DataAnnotations;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Login;

[Serializable]
[ProtoContract]
public class LoginRequest
{
    [ProtoMember(1)]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
    public string Email { get; set; } = null!;

    [ProtoMember(2)]
    public string Password { get; set; } = null!;
}