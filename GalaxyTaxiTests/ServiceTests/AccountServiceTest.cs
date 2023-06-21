using GalaxyTaxi.Api.Api;
using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Shared.Api.Models.Common;
using GalaxyTaxi.Shared.Api.Models.Login;
using GalaxyTaxi.Shared.Api.Models.Register;
using Microsoft.EntityFrameworkCore;
using Moq;
using ProtoBuf.Meta;

namespace GalaxyTaxiTests.ServiceTests
{
    [TestClass]
    public class AccountServiceTest
    {

        [TestMethod]
        public async Task RegisterTestAsync()
        {
            AccountService accountService = new AccountService(Helper.CreateDBContextInstance());
            RegisterRequest model = new RegisterRequest
            {
                CompanyEmail = "Test@gmail.com",
                CompanyName = "Test",
                Password = "password",
                Type = AccountType.CustomerCompany
            };


            var res = accountService.RegisterAsync(model);
            await res;

            Assert.IsNotNull(res);
            Assert.Equals(res.IsCompletedSuccessfully, true);
        }


        [TestMethod]
        public async Task LoginTestAsync()
        {
            AccountService accountService = new AccountService(Helper.CreateDBContextInstance());

            LoginRequest request = new LoginRequest
            {
                Email = "Test@gmail.com",
                Password = "password"
            };
            Task<LoginResponse> response = accountService.LoginAsync(request);
            await response;
            Assert.IsNotNull(response);
            Assert.Equals(response.IsCompletedSuccessfully, true);
        }

        [TestMethod]
        public void LogoutTest()
        {
            AccountService accountService = new AccountService(Helper.CreateDBContextInstance());
        }

        [TestMethod]
        public void ValidateEmailTest()
        {
        }

        [TestMethod]
        public void ValidateCompanyTest()
        {
        }
    }
}