namespace GalaxyTaxi.Api.Database.Models;

public class Employee
{
    public long Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Mobile { get; set; } = null!;

    public long CustomerCompanyId { get; set; }

    public long OfficeId { get; set; }

    public CustomerCompany CustomerCompany { get; set; } = null!;

    public Office Office { get; set; } = null!;

    public ICollection<EmployeeAddress> Addresses { get; set; } = null!;
}