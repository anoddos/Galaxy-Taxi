namespace GalaxyTaxi.Api.Database.Models;

public class CompanyDetail
{
    public long Id { get; set; }
    
    public double AmountPerUser { get; set; }

    public ICollection<Employee> Employees { get; set; }

    public ICollection<Office> Offices { get; set; }
    
    public CompanyDetail(long id, ICollection<Employee> employees, ICollection<Office> offices)
    {
        Id = id;
        Employees = employees;
        Offices = offices;
    }
}