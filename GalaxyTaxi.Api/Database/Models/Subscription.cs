using GalaxyTaxi.Shared.Api.Models.Common;

namespace GalaxyTaxi.Api.Database.Models;

public class Subscription
{
    public long Id { get; set; }
    
    public SubscriptionPlanType SubscriptionPlanTypeId { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
    
    public SubscriptionStatusId SubscriptionStatusId { get; set; }
}