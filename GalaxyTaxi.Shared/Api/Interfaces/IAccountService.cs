using GalaxyTaxi.Shared.Api.Models.Login;
using GalaxyTaxi.Shared.Api.Models.Register;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace GalaxyTaxi.Shared.Api.Interfaces;

[Service("Account")]
public interface IAccountService
{
    Task RegisterAsync(RegisterRequest request, CallContext context = default);
    Task ValidateEmailAsync(ValidateEmailRequest request, CallContext context = default);
    Task ValidateCompanyNameAsync(ValidateCompanyNameRequest request, CallContext context = default);
    Task<LoginResponse> LoginAsync(LoginRequest request, CallContext context = default);
    Task LogoutAsync(CallContext context = default);
}