using GalaxyTaxi.Shared.Api.Models.Common;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Subscription;

[ProtoContract]
[Serializable]
public class SubscriptionRequest
{
    [ProtoMember(1)]
    public SubscriptionPlanType SubscriptionPlanType { get; set; }
}