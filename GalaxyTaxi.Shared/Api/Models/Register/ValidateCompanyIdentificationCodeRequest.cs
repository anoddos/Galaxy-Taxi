using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Register;

[ProtoContract]
[Serializable]
public class ValidateCompanyIdentificationCodeRequest
{
    [ProtoMember(1)]
    public string IdentificationCode { get; set; } = null!;
}