using GalaxyTaxi.Shared.Api.Models.JourneyGenerator;
using GalaxyTaxi.Shared.Api.Models.VendorCompany;
using ProtoBuf;
namespace GalaxyTaxi.Shared.Api.Models.Auction;

[ProtoContract]
[Serializable]
public class AuctionInfo
{
    [ProtoMember(1)]
    public long Id  { get; set; }
    
    [ProtoMember(2)]
    public DateTime StartTime { get; set; }
    
    [ProtoMember(3)]
    public DateTime EndTime { get; set; }

    [ProtoMember(4)]
    public decimal Amount { get; set; }

    [ProtoMember(5)]
    public VendorCompanyInfo? CurrentWinner { get; set; }

    [ProtoMember(6)]
    public JourneyInfo JourneyInfo { get; set; } = null!;

    [ProtoMember(7)]
    public List<BidInfo> Bids { get; set; } = null!;
}