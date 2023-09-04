using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Auction;

[ProtoContract]
[Serializable]
public class GenerateAuctionsRequest
{
    [ProtoMember(1)] 
    public List<int> EmployeeIds { get; set; } = null!;
}