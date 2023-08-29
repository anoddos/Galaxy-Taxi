using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.CustomerCompany;

[ProtoContract]
[Serializable]
public class CustomerCompanyInfo
{
    [ProtoMember(1)]
    public long Id { get; set; }

    [ProtoMember(2)]
    public string Name { get; set; }
}