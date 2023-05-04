namespace GalaxyTaxi.Api.Database.Models;

public class Employee
{
    public long Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Mobile { get; set; } = null!;
}