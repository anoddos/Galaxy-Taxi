using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Auction;


[ProtoContract]
[Serializable]
public class UpdateFulfilmentPercentageRequest
{
    [ProtoMember(1)] 
    public long Id { get; set; }
    
    [ProtoMember(2)] 
    public double Percentage { get; set; }
}