namespace GalaxyTaxi.Shared.Api.Models.Common;

public static class SubscriptionMapping
{
    public static readonly Dictionary<SubscriptionPlanType, int> Mapping = new()
    {
        { SubscriptionPlanType.Weekly, 10 },
        { SubscriptionPlanType.Monthly, 100 },
        { SubscriptionPlanType.Annual, Int32.MaxValue}
    };
}