using Grpc.Net.Client;
using Grpc.Net.Client.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Http;
using ProtoBuf.Grpc.Client;

namespace GalaxyTaxi.Web.Extensions
{
    public static class ServiceCollectionGrpcExtensions
    {
        public static IServiceCollection AddGrpcChannel(this IServiceCollection services)
        {
            services.AddScoped(sp =>
            {
                var configuration = sp.GetRequiredService<IConfiguration>();
                var httpClient = new HttpClient(new GrpcWebHandler(GrpcWebMode.GrpcWeb, new CookieEnabledClientHandler()));
                var backendUrl = configuration["BackendUrl"] ?? sp.GetRequiredService<NavigationManager>().BaseUri;
                var channel = GrpcChannel.ForAddress(backendUrl, new GrpcChannelOptions {HttpClient = httpClient});

                return channel;
            });
            return services;
        }

        public static IServiceCollection AddGrpcServiceClient<TInterface>(this IServiceCollection services) where TInterface : class
        {
            services.AddScoped<TInterface>(sp =>
            {
                var channel = sp.GetRequiredService<GrpcChannel>();
                return channel.CreateGrpcService<TInterface>();
            });
            return services;
        }


        public class CookieEnabledClientHandler : HttpClientHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
                return base.SendAsync(request, cancellationToken);
            }
        }
    }
}