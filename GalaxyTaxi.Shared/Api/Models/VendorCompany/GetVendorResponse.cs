using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.VendorCompany;

[ProtoContract]
[Serializable]
public class GetVendorResponse
{
    [ProtoMember(1)]
    public List<VendorInfo> Vendors { get; set; }
}