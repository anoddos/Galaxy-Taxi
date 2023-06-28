using GalaxyTaxi.Api.Api;
using GalaxyTaxi.Shared.Api.Models.Common;
using GalaxyTaxi.Shared.Api.Models.Login;
using GalaxyTaxi.Shared.Api.Models.Register;

namespace GalaxyTaxiTests.ServiceTests;

[TestClass]
public class AccountServiceTest
{

    //[TestMethod]
    //public async Task RegisterTestAsync()
    //{
    //    var accountService = new AccountService(Helper.CreateDBContextInstance());
    //    var model = new RegisterRequest
    //    {
    //        CompanyEmail = "Test@gmail.com",
    //        CompanyName = "Test",
    //        Password = "password",
    //        Type = AccountType.CustomerCompany
    //    };


    //    var res = accountService.RegisterAsync(model);
    //    await res;

    //    Assert.IsNotNull(res);
    //    Assert.Equals(res.IsCompletedSuccessfully, true);
    //}


    //[TestMethod]
    //public async Task LoginTestAsync()
    //{
    //    var accountService = new AccountService(Helper.CreateDBContextInstance());

    //    var request = new LoginRequest
    //    {
    //        Email = "Test@gmail.com",
    //        Password = "password"
    //    };
    //    var response = accountService.LoginAsync(request);
    //    await response;
    //    Assert.IsNotNull(response);
    //    Assert.Equals(response.IsCompletedSuccessfully, true);
    //}

    //[TestMethod]
    //public void LogoutTest()
    //{
    //    var accountService = new AccountService(Helper.CreateDBContextInstance());
    //}

    [TestMethod]
    public void ValidateEmailTest()
    {
    }

    [TestMethod]
    public void ValidateCompanyTest()
    {
    }
}