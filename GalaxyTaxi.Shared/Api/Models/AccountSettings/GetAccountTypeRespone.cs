using GalaxyTaxi.Shared.Api.Models.Common;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.AccountSettings;

[ProtoContract]
[Serializable]
public class GetAccountTypeRespone
{
    [ProtoMember(1)]
    public AccountType AccountType { get; set; }
}