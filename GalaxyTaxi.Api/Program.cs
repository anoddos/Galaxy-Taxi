using GalaxyTaxi.Api.Api;
using GalaxyTaxi.Api.Database;
using GalaxyTaxi.Shared.Api.Interfaces;
using GrpcBrowser.Configuration;
using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc.Server;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;

services.AddCodeFirstGrpc();
services.AddCodeFirstGrpcReflection();
services.AddGrpcBrowser();

services.AddDbContext<Db>(options => { options.UseNpgsql(builder.Configuration.GetConnectionString("GalaxyTaxiDb")); });

services.AddCors(o =>
{
    o.AddPolicy("Default",
        policyBuilder =>
        {
            policyBuilder.SetIsOriginAllowed(_ => builder.Environment.IsDevelopment())
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
        });
    o.DefaultPolicyName = "Default";
});

var app = builder.Build();

using var scope = app.Services.CreateScope();
await scope.ServiceProvider.GetRequiredService<Db>().Database.MigrateAsync();

app.UseRouting();
app.UseCors();
app.UseGrpcBrowser();
app.UseGrpcWeb(new GrpcWebOptions() { DefaultEnabled = true });
app.UseEndpoints(options =>
{
    options.MapGrpcService<AccountService>().AddToGrpcBrowserWithService<IAccountService>();
    
    options.MapGrpcBrowser();
    options.MapCodeFirstGrpcReflectionService();
    options.MapGet("/", () => Results.Redirect("/grpc"));
});
app.Run();