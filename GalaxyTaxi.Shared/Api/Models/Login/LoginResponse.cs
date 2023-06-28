using GalaxyTaxi.Shared.Api.Models.Common;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Login;

[Serializable]
[ProtoContract]
public class LoginResponse
{
    [ProtoMember(1)]
    public AccountType LoggedInAs { get; set; }

    [ProtoMember(2)]
    public long AccountId { get; set; }
}