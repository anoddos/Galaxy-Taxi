using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.VendorCompany;

[ProtoContract]
[Serializable]
public class VendorInfo
{
    [ProtoMember(1)]
    public string Name { get; set; }
    [ProtoMember(2)]
    public DateTime? VerificationRequestDate { get; set; }
    [ProtoMember(3)]
    public bool IsVerified { get; set; }
    [ProtoMember(4)]
    public List<VendorFileModel> VendorFiles { get; set; } = new List<VendorFileModel>();

    [ProtoMember(5)]
    public string Email { get; set; }

    [ProtoMember(6)]
    public long VendorId { get; set; }
}
