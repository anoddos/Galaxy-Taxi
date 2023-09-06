using System.Security.Claims;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.AccountSettings;

[ProtoContract]
[Serializable]
public class GetAuthenticationStateProviderUserResponse
{
    [ProtoMember(1)] 
    public ClaimsPrincipal Principal { get; set; } = null!;
}