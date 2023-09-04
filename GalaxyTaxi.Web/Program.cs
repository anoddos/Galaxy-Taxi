using GalaxyTaxi.Shared.Api.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GalaxyTaxi.Web;
using MudBlazor.Services;
using GalaxyTaxi.Web.Extensions;
using OfficeOpenXml;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

builder.Services.AddMudServices();
builder.Services.AddMudBlazorDialog();
builder.Services.AddMudBlazorSnackbar();

builder.Services.AddGrpcChannel();
builder.Services.AddGrpcServiceClient<IAccountService>();
builder.Services.AddGrpcServiceClient<IAddressDetectionService>();
builder.Services.AddGrpcServiceClient<IAuctionService>();
builder.Services.AddGrpcServiceClient<IPaymentService>();
builder.Services.AddGrpcServiceClient<IEmployeeManagementService>();
builder.Services.AddGrpcServiceClient<ISubscriptionService>();
builder.Services.AddGrpcServiceClient<IOfficeManagementService>();

await builder.Build().RunAsync();