using ProtoBuf;
using GalaxyTaxi.Shared.Api.Models.Common;

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
    public bool IsVerified { get; set; }

    [ProtoMember(6)]
    public bool SupportTwoWayJourneys { get; set; }
}
