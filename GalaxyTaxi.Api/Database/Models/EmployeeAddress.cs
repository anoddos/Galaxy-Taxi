namespace GalaxyTaxi.Api.Database.Models;

public class EmployeeAddress
{
    public long Id { get; set; }

    public long AddressId { get; set; }

    public long EmployeeId { get; set; }

    public Address Address { get; set; } = null!;

    public Employee Employee { get; set; } = null!;

    public bool IsActive { get; set; }
}