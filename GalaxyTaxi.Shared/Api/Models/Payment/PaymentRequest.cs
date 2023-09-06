using GalaxyTaxi.Shared.Api.Models.Common;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Payment;

[ProtoContract]
[Serializable]
public class PaymentRequest
{
    [ProtoMember(1)]
    public SubscriptionPlanType SelectedSubscriptionType { get; set; }

    [ProtoMember(2)]
    public long Amount { get; set; }

    [ProtoMember(3)]
    public string token { get; set; }
}