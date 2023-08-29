using GalaxyTaxi.Shared.Api.Models.Register;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Auction;

[ProtoContract]
[Serializable]
public class BidInfo
{
    [ProtoMember(1)]
    public long Id { get; set; }
    
    [ProtoMember(2)]
    public double Amount { get; set; }
    
    [ProtoMember(3)]
    public DateTime TimeStamp { get; set; }
    
    [ProtoMember(4)]
    public AccountInfo Account { get; set; } = null!;
}