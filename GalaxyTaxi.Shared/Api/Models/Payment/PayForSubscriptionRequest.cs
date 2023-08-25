using GalaxyTaxi.Shared.Api.Models.Common;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Payment;

[ProtoContract]
[Serializable]
public class PayForSubscriptionRequest
{
    [ProtoMember(1)]
    public SubscriptionPlanType SelectedSubscriptionType { get; set; }
    
    [ProtoMember(2)]
    public decimal SubscriptionPrice { get; set; }
    
    [ProtoMember(3)]
    public string CardNumber { get; set; } = null!;

    [ProtoMember(4)]
    public string ExpirationDate { get; set; } = null!;

    [ProtoMember(5)]
    public string CVC { get; set; } = null!;
}