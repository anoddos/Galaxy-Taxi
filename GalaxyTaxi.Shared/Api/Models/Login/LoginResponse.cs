using GalaxyTaxi.Shared.Api.Models.Common;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Login;

[Serializable]
[ProtoContract]
public class LoginResponse
{
    public static AccountType LoggedInAs { get; set; }
}