using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.Login;
using GalaxyTaxi.Shared.Api.Models.Register;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Api.Api
{
    public class AccountService : IAccountService
    {
        private readonly Db _db;

        public AccountService(Db db)
        {
            _db = db;
        }
        
        public Task RegisterAsync(RegisterRequest request, CallContext context = default)
        {
            throw new NotImplementedException();
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
                throw new RpcException(new Status(StatusCode.AlreadyExists, "Email Already Taken"));
            }
        }

        public Task<LoginResponse> LoginAsync(LoginRequest request, CallContext context = default)
        {
            throw new NotImplementedException();
        }

        public Task LogoutAsync(CallContext context = default)
        {
            throw new NotImplementedException();
        }
    }
}