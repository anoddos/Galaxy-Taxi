using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Register;

[ProtoContract]
[Serializable]
public class ValidateCompanyNameRequest
{
    [ProtoMember(1)]
    public string CompanyName { get; set; } = null!;
}