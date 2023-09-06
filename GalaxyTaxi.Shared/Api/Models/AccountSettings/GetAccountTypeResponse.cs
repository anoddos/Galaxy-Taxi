using GalaxyTaxi.Shared.Api.Models.Common;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.AccountSettings;

[ProtoContract]
[Serializable]
public class GetAccountTypeResponse
{
    [ProtoMember(1)]
    public AccountType? AccountType { get; set; }
    
    
    [ProtoMember(2)]
    public AccountStatus AccountStatus { get; set; }
}