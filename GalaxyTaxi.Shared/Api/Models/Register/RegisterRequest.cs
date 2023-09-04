using System.ComponentModel.DataAnnotations;
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
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format.")]
    public string CompanyEmail { get; set; } = null!;

    [ProtoMember(3)]
    public string Password { get; set; } = null!;

    [ProtoMember(4)]
    public AccountType Type { get; set; }

    [ProtoMember(5)] 
    public string IdentificationCode { get; set; } = null!;
}
