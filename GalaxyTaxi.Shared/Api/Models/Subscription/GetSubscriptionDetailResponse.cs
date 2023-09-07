using GalaxyTaxi.Shared.Api.Models.Common;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Subscription;

[ProtoContract]
[Serializable]
public class GetSubscriptionDetailResponse
{
    [ProtoMember(1)]
    public SubscriptionPlanType SubscriptionPlanType { get; set; }

    [ProtoMember(2)]
    public decimal Price { get; set; }

    [ProtoMember(3)]
    public SubscriptionStatus Status { get; set; }
}