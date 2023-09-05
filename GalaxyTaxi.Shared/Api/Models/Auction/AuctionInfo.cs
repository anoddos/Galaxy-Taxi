using GalaxyTaxi.Shared.Api.Models.Common;
using GalaxyTaxi.Shared.Api.Models.JourneyGenerator;
using GalaxyTaxi.Shared.Api.Models.VendorCompany;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Auction;

[ProtoContract]
[Serializable]
public class AuctionInfo
{
    [ProtoMember(1)] 
    public long Id { get; set; }

    [ProtoMember(2)] 
    public DateTime StartTime { get; set; }

    [ProtoMember(3)] 
    public DateTime EndTime { get; set; }

    [ProtoMember(4)]
    public double Amount { get; set; }

    [ProtoMember(5)] 
    public VendorCompanyInfo? CurrentWinner { get; set; }

    [ProtoMember(6)] 
    public JourneyInfo JourneyInfo { get; set; } = null!;

    [ProtoMember(7)] 
    public IEnumerable<BidInfo> Bids { get; set; } = null!;

    [ProtoMember(8)] 
    public DateTime FromDate { get; set; }

    [ProtoMember(9)] 
    public DateTime ToDate { get; set; }

    [ProtoMember(10)] 
    public Feedback? Feedback { get; set; }

    [ProtoMember(11)] 
    public string? Comment { get; set; } = null!;
}