using System.Security.Claims;
using GalaxyTaxi.Shared.Api.Models.AccountSettings;
using GalaxyTaxi.Shared.Api.Models.VendorCompany;
using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using GalaxyTaxi.Shared.Api.Models.Login;
using GalaxyTaxi.Shared.Api.Models.Register;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;
using GalaxyTaxi.Shared.Api.Models.Admin;
using GalaxyTaxi.Shared.Api.Models.Filters;

namespace GalaxyTaxi.Shared.Api.Interfaces;

[Service("Account")]
public interface IAccountService
{
    Task RegisterAsync(RegisterRequest request, CallContext context = default);
    Task ValidateEmailAsync(ValidateEmailRequest request, CallContext context = default);
    Task ValidateCompanyNameAsync(ValidateCompanyNameRequest request, CallContext context = default);
    Task ValidateCompanyIdentificationCodeAsync(ValidateCompanyIdentificationCodeRequest request, CallContext context = default);
    Task<LoginResponse> LoginAsync(LoginRequest request, CallContext context = default);
    Task LogoutAsync(CallContext context = default);
    Task<IsLoggedInResponse> IsLoggedIn(CallContext context = default);
    Task UpdateAccountSettings(UpdateAccountSettingsRequest request, CallContext context = default);
    Task<AccountSettings?> GetAccountSettings(CallContext context = default);
    Task<GetAccountTypeResponse> GetAccountType(CallContext context = default);
    Task<GetAuthenticationStateProviderUserResponse> GetAuthenticationStateProviderUserAsync(CallContext context = default);
    Task<GetVendorFilesResponse> GetVendorFiles(GetVendorFilesRequest request,  CallContext context = default);
    Task RespondToVendor(RespondToVendorRequest request, CallContext context = default);
    Task<GetVendorResponse> GetVendorCompanies(VendorFilter vendorFilter);
}