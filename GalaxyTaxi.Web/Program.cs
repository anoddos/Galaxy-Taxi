using GalaxyTaxi.Shared.Api.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GalaxyTaxi.Web;
using MudBlazor.Services;
using GalaxyTaxi.Web.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddMudBlazorDialog();
builder.Services.AddMudBlazorSnackbar();

builder.Services.AddGrpcChannel();
builder.Services.AddGrpcServiceClient<IAccountService>();

await builder.Build().RunAsync();