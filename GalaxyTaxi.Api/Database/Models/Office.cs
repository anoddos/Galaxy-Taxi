using ProtoBuf.WellKnownTypes;

namespace GalaxyTaxi.Api.Database.Models;

public class Office
{
    public long Id { get; set; }

    public long AddressId { get; set; }

    public long CustomerCompanyId { get; set; }

    public TimeSpan WorkingStartTime { get; set; }

    public TimeSpan WorkingEndTime { get; set; }

    public Address Address { get; set; } = null!;

    public CustomerCompany CustomerCompany { get; set; } = null!;

    public ICollection<Employee> Employees { get; set; } = null!;
}