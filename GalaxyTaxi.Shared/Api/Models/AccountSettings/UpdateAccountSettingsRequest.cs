using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaxyTaxi.Shared.Api.Models.VendorCompany;

namespace GalaxyTaxi.Shared.Api.Models.AccountSettings;
[ProtoContract]
[Serializable]
public class UpdateAccountSettingsRequest
{
    [ProtoMember(1)]
    public string OldPassword { get; set; } = string.Empty;

    [ProtoMember(2)]
    public string NewPassword { get; set; } = string.Empty;

    [ProtoMember(3)]
    public AccountSettings? AccountInformation { get; set; }
    
    [ProtoMember(4)]
    public List<VendorFileModel> Files { get; set; }
}
