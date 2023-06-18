namespace GalaxyTaxi.Api.Database.Models;

public class Stop
{
    public long Id { get; set; }

    public long JourneyId { get; set; }

    public long EmployeeAddressId { get; set; }

    public EmployeeAddress EmployeeAddress { get; set; } = null!;

    public Journey Journey { get; set; } = null!;
}