using GalaxyTaxi.Shared.Api.Models.Common;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Register;

[ProtoContract]
[Serializable]
public class AccountInfo
{
    [ProtoMember(1)]
    public long Id { get; set; }

    [ProtoMember(2)]
    public AccountType AccountTypeId { get; set; }
    
    [ProtoMember(3)]
    public string Email { get; set; } = null!;
    
    [ProtoMember(4)]
    public string CompanyName { get; set; } = null!;
    
    [ProtoMember(5)]
    public string PasswordHash { get; set; } = null!;
}