using GalaxyTaxi.Api.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace GalaxyTaxi.Api.Database;
public class Db : DbContext
{
    public DbSet<Account> Accounts { get; set; } = null!;
    
    public DbSet<Address> Addresses { get; set; } = null!;
    
    public DbSet<CustomerCompany> CustomerCompanies { get; set; } = null!;
    
    public DbSet<VendorCompany> VendorCompanies { get; set; } = null!;
    
    public DbSet<Employee> Employees { get; set; } = null!;
    
    public DbSet<EmployeeAddress> EmployeeAddresses { get; set; } = null!;
    
    public DbSet<Office> Offices { get; set; } = null!;
    
    public DbSet<Stop> Stops { get; set; } = null!;

    public DbSet<Journey> Journeys { get; set; } = null!;
    
    public DbSet<Auction> Auctions { get; set; } = null!;
    
    public DbSet<Bid> Bids { get; set; } = null!;
    
    public Db(DbContextOptions options) : base(options)
    {
        
    }
}