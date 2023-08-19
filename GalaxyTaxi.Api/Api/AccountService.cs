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

        try
        {
            await _db.Accounts.AddAsync(account);

            await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, "Could not insert into db"));
        }

        SetSessionValue("AccountId", account.Id.ToString());
        SetSessionValue("LoggedInAs", account.AccountTypeId.ToString());
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

        SetSessionValue("AccountId", account.Id.ToString());
        SetSessionValue("LoggedInAs", account.AccountTypeId.ToString());

        return new LoginResponse
        {
            LoggedInAs = account.AccountTypeId,
            AccountId = account.Id
        };
    }

    public async Task LogoutAsync( CallContext context = default)
    {
        // Get the user ID from the session
        var accountId = GetSessionValue("AccountId");

        if (string.IsNullOrWhiteSpace(accountId))
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Not Logged In"));
        }

        //Clear the user ID from the session

        ClearSessionValue("AccountId");
    }
    
    private void SetSessionValue(string key, string value)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        httpContext?.Session.SetString(key, value);
    }

    private string GetSessionValue(string key)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        return httpContext?.Session.GetString(key) ?? "";
    }

    private void ClearSessionValue(string key)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        httpContext?.Session.Remove(key);
    }

    private string SaltAndHashPassword(string password)
    {
        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
        return hashedPassword;
    }
}