using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.VendorCompany;

[ProtoContract]
[Serializable]
public class VendorCompanyInfo
{
    [ProtoMember(1)]
    public long Id { get; set; }

    [ProtoMember(2)]
    public string Name { get; set; } = null!;
}