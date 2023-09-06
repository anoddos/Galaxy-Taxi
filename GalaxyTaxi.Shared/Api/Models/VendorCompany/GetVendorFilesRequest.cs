using System.ComponentModel.DataAnnotations;
using ProtoBuf;


namespace GalaxyTaxi.Shared.Api.Models.VendorCompany;

[ProtoContract]
[Serializable]
public class GetVendorFilesRequest
{
    [ProtoMember(1)]
    public long VendorId { get; set; }
}
