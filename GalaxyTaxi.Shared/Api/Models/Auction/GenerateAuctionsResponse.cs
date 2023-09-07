using GalaxyTaxi.Shared.Api.Models.Common;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Auction;

[ProtoContract]
[Serializable]
public class GenerateAuctionsResponse
{
    [ProtoMember(1)] 
    public int GeneratedAuctionCount { get; set; }
    
    
    [ProtoMember(2)] 
    public double GeneratedAuctionTotalCost { get; set; }
    
    
    [ProtoMember(3)] 
    public int AffectedUsersCount { get; set; }
}