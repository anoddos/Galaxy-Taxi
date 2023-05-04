namespace GalaxyTaxi.Api.Database.Models;

public class CustomerDetails
{
    public long Id { get; set; }
    
    public double AmountPerUser { get; set; }

    public ICollection<Employee> Employees { get; set; }

    public ICollection<Office> Offices { get; set; }
}