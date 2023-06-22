using GalaxyTaxi.Shared.Api.Models.Common;

namespace GalaxyTaxi.Api.Database.Models;

public class CustomerCompany
{
    public long Id { get; set; }

    public string Name { get; set; } = null!;

    public string IdentificationCode { get; set; } = null!;

    public double MaxAmountPerEmployee { get; set; }

    public long AccountId { get; set; }
    
    public long SubscriptionId { get; set; }

    public Account Account { get; set; } = null!;

    public Subscription Subscription { get; set; } = null!;

    public ICollection<Employee> Employees { get; set; } = null!;

    public ICollection<Office> Offices { get; set; } = null!;
}