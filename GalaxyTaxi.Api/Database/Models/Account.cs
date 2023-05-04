using GalaxyTaxi.Shared.Api.Models.Common;

namespace GalaxyTaxi.Api.Database.Models
{
    public class Account
    {
        public Guid Id { get; set; }
        
        public AccountType AccountType { get; set; }
        public string Email { get; set; } = null!;
        public string CompanyName { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;
    }
}