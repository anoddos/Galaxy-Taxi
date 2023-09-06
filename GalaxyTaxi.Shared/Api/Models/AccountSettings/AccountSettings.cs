using ProtoBuf;
using GalaxyTaxi.Shared.Api.Models.Common;
using GalaxyTaxi.Shared.Api.Models.VendorCompany;

namespace GalaxyTaxi.Shared.Api.Models.AccountSettings;

[ProtoContract]
[Serializable]
public class AccountSettings
{
    [ProtoMember(1)]
    public string CompanyName { get; set; }
    
    [ProtoMember(2)]
    public string Email { get; set; }
    
    [ProtoMember(3)]
    public double MaxAmountPerEmployee { get; set; }
    
    [ProtoMember(4)]
    public AccountType AccountType { get; set; }
    
    [ProtoMember(5)]
    public AccountStatus Status { get; set; }

    [ProtoMember(6)]
    public bool SupportTwoWayJourneys { get; set; }

    [ProtoMember(7)]
    public List<VendorFileModel> VendorFiles { get; set; } = new List<VendorFileModel>();

}
