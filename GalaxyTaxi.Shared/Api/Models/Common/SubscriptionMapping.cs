namespace GalaxyTaxi.Shared.Api.Models.Common;

public static class SubscriptionMapping
{
    public static readonly Dictionary<SubscriptionPlanType, int> EmployeeMapping = new()
    {
        { SubscriptionPlanType.Weekly, 10 },
        { SubscriptionPlanType.Monthly, 100 },
        { SubscriptionPlanType.Annual, Int32.MaxValue}
    };

	public static readonly Dictionary<SubscriptionPlanType, int> AmountMapping = new()
	{
		{ SubscriptionPlanType.Weekly, 20 },
		{ SubscriptionPlanType.Monthly, 60 },
		{ SubscriptionPlanType.Annual, 500}
	};
}