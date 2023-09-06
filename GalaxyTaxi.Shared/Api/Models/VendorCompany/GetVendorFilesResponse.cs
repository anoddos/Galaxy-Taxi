using System.ComponentModel.DataAnnotations;
using ProtoBuf;


namespace GalaxyTaxi.Shared.Api.Models.VendorCompany;

[ProtoContract]
[Serializable]
public class GetVendorFilesResponse
{
    [ProtoMember(1)]
    public List<VendorFileModel> VendorFiles { get; set; }
}
