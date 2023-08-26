using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Api.Database.Models;
using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.Login;
using GalaxyTaxi.Shared.Api.Models.Register;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;
using System.Security.Cryptography;
using BCrypt.Net;
using GalaxyTaxi.Shared.Api.Models.Common;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace GalaxyTaxi.Api.Api;

public class AccountService : IAccountService
{
    private readonly Db _db;
    private readonly IHttpContextAccessor _httpContextAccessor;


	public AccountService(Db db, IHttpContextAccessor httpContextAccessor)
    {
        _db = db;
        _httpContextAccessor = httpContextAccessor;

	}
	private async Task LoginSession(AccountType accountType, long accountId, long companyId, CallContext context = default)
	{
		var httpContext = _httpContextAccessor.HttpContext;
		var principal = new ClaimsPrincipal();
		var claims = new List<Claim>()
		{
			new Claim(AuthenticationKey.LoggedInAs, accountType.ToString()),
			new Claim(AuthenticationKey.AccountId, accountId.ToString())
		};
		if (accountType != AccountType.Admin)
		{
			claims.Add(new Claim(AuthenticationKey.CompanyId, companyId.ToString()));
		}
		var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
		principal.AddIdentity(claimsIdentity);
		await httpContext?.SignInAsync(principal);
	}

	private string GetSessionValue(string key, CallContext context = default)
	{
		var httpContext = _httpContextAccessor.HttpContext;
        var res = httpContext?.User.Claims.FirstOrDefault(c => c.Type == key);

        return res == null ? "" : res.Value.ToString();
	}

	private async Task ClearSession(CallContext context = default)
	{
		var httpContext = _httpContextAccessor.HttpContext;
		await httpContext?.SignOutAsync();
	}

	public async Task RegisterAsync(RegisterRequest request, CallContext context = default)
    {
        await ValidateEmailAsync(new ValidateEmailRequest { CompanyEmail = request.CompanyEmail });
        await ValidateCompanyNameAsync(new ValidateCompanyNameRequest{ CompanyName = request.CompanyName});

        var account = new Account
        {
            CompanyName = request.CompanyName,
            Email = request.CompanyEmail,
            PasswordHash = SaltAndHashPassword(request.Password),
            AccountTypeId = request.Type
        };
        long companyId = 0;
        
        try
        {
            var addedAccount = await _db.Accounts.AddAsync(account);
            
            await _db.SaveChangesAsync();
            if (request.Type == AccountType.CustomerCompany)
            {
                var customerCompany = new CustomerCompany
                {
                    AccountId = addedAccount.Entity.Id,
                    Name = request.CompanyName,
                    MaxAmountPerEmployee = 0,
                    IdentificationCode = "sdf"
                };
                await _db.CustomerCompanies.AddAsync(customerCompany);
                companyId = customerCompany.Id;
            }
            else
            {
                var vendorCompany = new VendorCompany
                {
                    AccountId = addedAccount.Entity.Id,
                    Name = request.CompanyName,
                };
                await _db.VendorCompanies.AddAsync(vendorCompany);
                companyId = vendorCompany.Id;

            }

            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, "Could not insert into db"));
        }

        await LoginSession(account.AccountTypeId, account.Id, companyId);
    }

    public async Task ValidateEmailAsync(ValidateEmailRequest request, CallContext context = default)
    {
        var alreadyExists =  await _db.Accounts.AnyAsync(x => x.Email == request.CompanyEmail);
        if (alreadyExists)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "Email Already Taken"));
        }
    }

    public async Task ValidateCompanyNameAsync(ValidateCompanyNameRequest request, CallContext context = default)
    {
        var alreadyExists =  await _db.Accounts.AnyAsync(x => x.CompanyName == request.CompanyName);
        if (alreadyExists)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "Company Name Already Exists"));
        }
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CallContext context = default)
    {
        var account = await _db.Accounts.SingleOrDefaultAsync(x => x.Email == request.Email);


        if (account == null || (account != null && !BCrypt.Net.BCrypt.Verify(request.Password, account.PasswordHash)))
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Wrong email or password"));
        }
        long companyId = 0;
        if (account?.AccountTypeId == AccountType.CustomerCompany)
        {
            var company = await _db.CustomerCompanies.FirstOrDefaultAsync(c => c.AccountId == account.Id);
            companyId = company.Id;
        }
        else if (account?.AccountTypeId == AccountType.VendorCompany)
        {
            var company = await _db.VendorCompanies.FirstOrDefaultAsync(c => c.AccountId == account.Id);
            companyId = company.Id;
        }
        await LoginSession(account.AccountTypeId, account.Id, companyId);
        return new LoginResponse
        {
            LoggedInAs = account.AccountTypeId,
            AccountId = account.Id
        };
    }

    public async Task LogoutAsync( CallContext context = default)
    {
        // Get the user ID from the session
        var accountId = GetSessionValue(AuthenticationKey.AccountId);

        if (string.IsNullOrWhiteSpace(accountId))
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Not Logged In"));
        }

        await ClearSession();
    }
    


    private string SaltAndHashPassword(string password)
    {
        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
        return hashedPassword;
    }
}