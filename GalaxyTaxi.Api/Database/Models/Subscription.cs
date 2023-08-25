using GalaxyTaxi.Shared.Api.Models.Common;

namespace GalaxyTaxi.Api.Database.Models;

public class Subscription
{
    public long Id { get; set; }

    public long CustomerCompanyId { get; set; }
    
    public SubscriptionPlanType SubscriptionPlanTypeId { get; set; }
    
    public DateTime? StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public DateTime? DeactivationTime { get; set; }
    
    public SubscriptionStatus SubscriptionStatus { get; set; }

    public CustomerCompany CustomerCompany { get; set; } = null!;
}