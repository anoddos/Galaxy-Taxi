namespace GalaxyTaxi.Api.Database.Models;

public class Destination
{
    public long Id { get; set; }
    
    public long JourneyId { get; set; }
    
    public long EmployeeAddressId { get; set; }

    public EmployeeAddress EmployeeAddress { get; set; }

    public Journey Journey { get; set; }
}