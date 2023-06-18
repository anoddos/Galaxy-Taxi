namespace GalaxyTaxi.Api.Database.Models;

public class Office
{
    public long Id { get; set; }

    public long AddressId { get; set; }

    public long CustomerCompanyId { get; set; }

    public DateTime WorkingStartTime { get; set; }

    public DateTime WorkingEndTime { get; set; }

    public Address Address { get; set; } = null!;

    public CustomerCompany CustomerCompany { get; set; } = null!;

    public ICollection<Employee> Employees { get; set; } = null!;
}