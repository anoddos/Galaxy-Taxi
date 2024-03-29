﻿using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Api.Database.Models;
using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.Login;
using GalaxyTaxi.Shared.Api.Models.Register;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;
using GalaxyTaxi.Shared.Api.Models.Common;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using GalaxyTaxi.Shared.Api.Models.AccountSettings;
using GalaxyTaxi.Shared.Api.Models.VendorCompany;
using GalaxyTaxi.Shared.Api.Models.Admin;
using GalaxyTaxi.Shared.Api.Models.Filters;
using GalaxyTaxi.Shared.Api.Models.Payment;
using GalaxyTaxi.Shared.Api.Models.Subscription;

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
        if (request.Type != AccountType.CustomerCompany && request.Type != AccountType.VendorCompany)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "Invalid Account Type"));
        }

        await ValidateEmailAsync(new ValidateEmailRequest { CompanyEmail = request.CompanyEmail });
        await ValidateCompanyNameAsync(new ValidateCompanyNameRequest { CompanyName = request.CompanyName });

        if (request.Type == AccountType.VendorCompany)
            await ValidateCompanyIdentificationCodeAsync(new ValidateCompanyIdentificationCodeRequest
                { IdentificationCode = request.IdentificationCode });

        var account = new Account
        {
            CompanyName = request.CompanyName,
            Email = request.CompanyEmail,
            PasswordHash = SaltAndHashPassword(request.Password),
            AccountTypeId = request.Type,
            Status = request.Type == AccountType.CustomerCompany ? AccountStatus.Verified : AccountStatus.Registered
        };
        long companyId;

        try
        {
            var addedAccount = await _db.Accounts.AddAsync(account);

            await _db.SaveChangesAsync();
            CustomerCompany customerCompany = null;
            VendorCompany vendorCompany = null;

            if (request.Type == AccountType.CustomerCompany)
            {
                customerCompany = new CustomerCompany
                {
                    AccountId = addedAccount.Entity.Id,
                    Name = request.CompanyName,
                    MaxAmountPerEmployee = 5,
                    SupportTwoWayJourneys = false
                };
                await _db.CustomerCompanies.AddAsync(customerCompany);
            }
            else
            {
                vendorCompany = new VendorCompany
                {
                    AccountId = addedAccount.Entity.Id,
                    Name = request.CompanyName,
                    IdentificationCode = request.IdentificationCode
                };
                await _db.VendorCompanies.AddAsync(vendorCompany);
                companyId = vendorCompany.Id;
            }

            await _db.SaveChangesAsync();

            companyId = request.Type == AccountType.CustomerCompany ? customerCompany.Id : vendorCompany.Id;
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Internal, "Could not insert into db"));
        }

        await LoginSession(account.AccountTypeId, account.Id, companyId);
    }

    public async Task ValidateEmailAsync(ValidateEmailRequest request, CallContext context = default)
    {
        var alreadyExists = await _db.Accounts.AnyAsync(x => x.Email == request.CompanyEmail);
        if (alreadyExists)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "Email is Already Taken"));
        }
    }

    public async Task ValidateCompanyNameAsync(ValidateCompanyNameRequest request, CallContext context = default)
    {
        var alreadyExists = await _db.Accounts.AnyAsync(x => x.CompanyName == request.CompanyName);
        if (alreadyExists)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "Company Name Already Exists"));
        }
    }

    public async Task ValidateCompanyIdentificationCodeAsync(ValidateCompanyIdentificationCodeRequest request,
        CallContext context = default)
    {
        var alreadyExists = await _db.VendorCompanies.AnyAsync(x => x.IdentificationCode == request.IdentificationCode);
        if (alreadyExists)
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, "Identification Code Name Already Exists"));
        }
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CallContext context = default)
    {
        var account = await _db.Accounts.SingleOrDefaultAsync(x => x.Email == request.Email);

        if (account == null || (account != null && !BCrypt.Net.BCrypt.Verify(request.Password, account.PasswordHash)))
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Wrong email or password"));
        }

        long companyId = default;

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

    public async Task LogoutAsync(CallContext context = default)
    {
        // Get the user ID from the session
        var accountId = GetSessionValue(AuthenticationKey.AccountId);

        if (string.IsNullOrWhiteSpace(accountId))
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Not Logged In"));
        }

        await ClearSession();
    }

    private static string SaltAndHashPassword(string password)
    {
        var salt = BCrypt.Net.BCrypt.GenerateSalt();
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
        return hashedPassword;
    }

    private async Task LoginSession(AccountType accountType, long accountId, long companyId)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var principal = new ClaimsPrincipal();
        var claims = new List<Claim>
        {
            new(AuthenticationKey.LoggedInAs, accountType.ToString()),
            new(AuthenticationKey.AccountId, accountId.ToString())
        };

        if (accountType != AccountType.Admin)
        {
            claims.Add(new Claim(AuthenticationKey.CompanyId, companyId.ToString()));
        }

        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
        principal.AddIdentity(claimsIdentity);

        await httpContext?.SignInAsync(principal);
    }

    private string GetSessionValue(string key)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var res = httpContext?.User.Claims.FirstOrDefault(c => c.Type == key);

        return res == null ? "" : res.Value;
    }

    private async Task ClearSession()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        await httpContext?.SignOutAsync();
    }

    public Task<IsLoggedInResponse> IsLoggedIn(CallContext context = default)
    {
        var loggedInAs = GetSessionValue(AuthenticationKey.LoggedInAs);
        return Task.FromResult(new IsLoggedInResponse { IsLoggedIn = !string.IsNullOrEmpty(loggedInAs) });
    }

    private async Task ValidateNewPassword(string OldPassword, string NewPassword, CallContext context = default)
    {
        var accountId = long.Parse(GetSessionValue(AuthenticationKey.AccountId) ?? "-1");

        if (accountId == -1) throw new RpcException(new Status(StatusCode.NotFound, "Not Logged In"));


        var oldPasswordEncrypt = SaltAndHashPassword(OldPassword);

        var account = await _db.Accounts.SingleOrDefaultAsync(a => a.Id == accountId);
        if (!BCrypt.Net.BCrypt.Verify(OldPassword, account.PasswordHash))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Old Password Is Incorrect"));
        if (BCrypt.Net.BCrypt.Verify(NewPassword, account.PasswordHash))
            throw new RpcException(new Status(StatusCode.AlreadyExists,
                "New Password Can't be the same as the old password"));
    }

    public async Task<AccountSettings> UpdateAccountSettings(UpdateAccountSettingsRequest request, CallContext context = default)
    {
        var accountId = long.Parse(GetSessionValue(AuthenticationKey.AccountId) ?? "-1");
        if (accountId == -1) throw new RpcException(new Status(StatusCode.NotFound, "Not Logged In"));

        var account = await _db.Accounts.SingleOrDefaultAsync(a => a.Id == accountId);

        if (!string.IsNullOrWhiteSpace(request.AccountInformation?.CompanyName) &&
            request.AccountInformation?.CompanyName != account?.CompanyName)
        {
            account.CompanyName = request.AccountInformation.CompanyName;
        }

        Enum.TryParse(GetSessionValue(AuthenticationKey.LoggedInAs), out AccountType loggedInAs);

        if (request.AccountInformation?.MaxAmountPerEmployee != null && loggedInAs == AccountType.CustomerCompany)
        {
            var company = await _db.CustomerCompanies.SingleOrDefaultAsync(c => c.AccountId == accountId);
            if (request.AccountInformation?.MaxAmountPerEmployee != company.MaxAmountPerEmployee)
            {
                company.MaxAmountPerEmployee = (double)request.AccountInformation?.MaxAmountPerEmployee;
            }

            company.SupportTwoWayJourneys = request.AccountInformation.SupportTwoWayJourneys;
        }

        if (!string.IsNullOrWhiteSpace(request.AccountInformation?.Email) &&
            request.AccountInformation?.Email != account?.Email)
        {
            account.Email = request.AccountInformation.Email;
        }

        await _db.SaveChangesAsync();

        if (request.Files.Any())
        {
            if (loggedInAs != AccountType.VendorCompany)
                throw new RpcException(new Status(StatusCode.Unavailable,
                    "File Upload Only Allowed for Vendor Companies"));

            await UploadVendorFiles(request.Files);

            account.VerificationRequestDate = DateTime.UtcNow;
            account.Status = AccountStatus.Pending;
            if (request.AccountInformation != null)
            {
                request.AccountInformation.Status = AccountStatus.Pending;
            }

            await _db.SaveChangesAsync();
        }

        if (!string.IsNullOrEmpty(request.NewPassword) && !string.IsNullOrEmpty(request.OldPassword))
        {
            await ValidateNewPassword(request.OldPassword, request.NewPassword);
            account.PasswordHash = SaltAndHashPassword(request.NewPassword);
            await _db.SaveChangesAsync();
        }
        return request.AccountInformation ?? new AccountSettings();
    }

    private async Task UploadVendorFiles(List<VendorFileModel> files)
    {
        var companyId = GetCompanyId();

        foreach (var file in files)
        {
            var vendorFile = new VendorFile
            {
                Name = file.Name,
                Path = file.Path,
                UploadDate = DateTime.UtcNow,
                VendorCompanyId = companyId
            };
            await _db.VendorFiles.AddAsync(vendorFile);
        }

        await _db.SaveChangesAsync();
    }

    public async Task<AccountSettings?> GetAccountSettings(CallContext context = default)
    {
        var accountId = long.Parse(GetSessionValue(AuthenticationKey.AccountId) ?? "-1");

        if (accountId == -1) throw new RpcException(new Status(StatusCode.NotFound, "Not Logged In"));

        Enum.TryParse(GetSessionValue(AuthenticationKey.LoggedInAs), out AccountType loggedInAs);
        var maxAmountPerEmployee = 0.0;
        var supportTwoWayJourneys = false;
        var files = new List<VendorFileModel>();


        var subcsription = new GetSubscriptionDetailResponse { Status = SubscriptionStatus.InActive };

        if (loggedInAs == AccountType.CustomerCompany)
        {
            var company = await _db.CustomerCompanies.SingleOrDefaultAsync(a => a.AccountId == accountId);
            if (company == null)
                throw new RpcException(new Status(StatusCode.NotFound, "Customer company does not exists"));
            maxAmountPerEmployee = company.MaxAmountPerEmployee;
            supportTwoWayJourneys = company.SupportTwoWayJourneys;
            var subscriptionInDb = await _db.Subscriptions.SingleOrDefaultAsync(x => x.CustomerCompanyId == GetCompanyId());
            if (subscriptionInDb != null)
            {
                subcsription.SubscriptionPlanType = subscriptionInDb.SubscriptionPlanTypeId;
                subcsription.Status = subscriptionInDb.SubscriptionStatus;
                subcsription.Price = SubscriptionMapping.AmountMapping[subscriptionInDb.SubscriptionPlanTypeId];
            } 
            
            //subscriptionType = company.sub
        }
        else if (loggedInAs == AccountType.VendorCompany)
        {
            var companyId = long.Parse(GetSessionValue(AuthenticationKey.CompanyId) ?? "-1");
            var response = await GetVendorFiles(new GetVendorFilesRequest { VendorId = companyId });
            if (response != null)
            {
                files = response.VendorFiles;
            }
        }

        var account = await _db.Accounts.SingleOrDefaultAsync(a => a.Id == accountId);

        return account != null
            ? new AccountSettings
            {
                CompanyName = account.CompanyName,
                Email = account.Email,
                MaxAmountPerEmployee = maxAmountPerEmployee,
                AccountType = loggedInAs,
                SupportTwoWayJourneys = supportTwoWayJourneys,
                Status = account.Status,
                VendorFiles = files,
                SubscriptionInfo = subcsription
            }
            : null;
    }

    public async Task<GetAccountTypeResponse> GetAccountType(CallContext context = default)
    {
        if (Enum.TryParse(GetSessionValue(AuthenticationKey.LoggedInAs), out AccountType loggedInAs))
        {
            var status = AccountStatus.Verified;
            if (loggedInAs == AccountType.VendorCompany)
            {
                var accountId = GetAccountId();
                status = (await _db.Accounts.SingleAsync(x => x.Id == accountId)).Status;
            }

            return new GetAccountTypeResponse
            {
                AccountType = loggedInAs,
                AccountStatus = status
            };
        }

        return new GetAccountTypeResponse
        {
            AccountType = null
        };
    }

    public async Task<GetAuthenticationStateProviderUserResponse> GetAuthenticationStateProviderUserAsync(
        CallContext context = default)
    {
        var httpContext = _httpContextAccessor.HttpContext;
        var res = httpContext?.User;
        return await Task.FromResult(new GetAuthenticationStateProviderUserResponse
        {
            Principal = httpContext?.User
        });
    }


    public async Task<GetVendorFilesResponse> GetVendorFiles(GetVendorFilesRequest request, CallContext context = default)
    {
        Enum.TryParse(GetSessionValue(AuthenticationKey.LoggedInAs), out AccountType loggedInAs);

        var files = await _db.VendorFiles.Include(e => e.VendorCompany)
            .Where(v => v.VendorCompanyId == request.VendorId).Select(e => new VendorFileModel
            {
                Email = e.VendorCompany.Account.Email,
                Name = e.Name,
                Path = e.Path,
                UploadDate = e.UploadDate
            }).ToListAsync();
        return new GetVendorFilesResponse { VendorFiles = files };
    }

    public async Task RespondToVendor(RespondToVendorRequest request, CallContext context = default)
    {
        Enum.TryParse(GetSessionValue(AuthenticationKey.LoggedInAs), out AccountType loggedInAs);
        if (loggedInAs != AccountType.Admin)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument,
                "This type of operations is only allowed for Admin"));
        }

        var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Email == request.VendorEmail);
        account.Status = request.NewStatus;
        await _db.SaveChangesAsync();
    }

    public async Task<GetVendorResponse> GetVendorCompanies(VendorFilter vendorFilter)
    {
        var vendors = _db.VendorCompanies.Where(x => x.Account.Status == vendorFilter.Status);

        return new GetVendorResponse
        {
            Vendors = await vendors.Select(x => new VendorInfo
            {
                VendorId = x.Id,
                Name = x.Name,
                Status = x.Account.Status,
                VerificationRequestDate = x.Account.VerificationRequestDate,
                Email = x.Account.Email
            }).ToListAsync()
        };
    }

    private long GetCompanyId()
    {
        var companyId = GetSessionValue(AuthenticationKey.CompanyId);

        if (string.IsNullOrWhiteSpace(companyId))
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Not Logged In"));
        }

        return long.Parse(companyId);
    }

    private long GetAccountId()
    {
        var accountId = GetSessionValue(AuthenticationKey.AccountId);

        if (string.IsNullOrWhiteSpace(accountId))
        {
            throw new RpcException(new Status(StatusCode.NotFound, "Not Logged In"));
        }

        return long.Parse(accountId);
    }

	public async Task UpdatePaymentToken(UpdatePaymentTokenRequest request, CallContext context = default)
	{
		var accountId = GetSessionValue(AuthenticationKey.AccountId);

		if (string.IsNullOrWhiteSpace(accountId))
		{
			throw new RpcException(new Status(StatusCode.NotFound, "Not Logged In"));
		}

        var account = await _db.Accounts.FirstOrDefaultAsync(a => a.Id == long.Parse(accountId));
        account.PaymentToken = request.Token;
        await _db.SaveChangesAsync();
	}
}