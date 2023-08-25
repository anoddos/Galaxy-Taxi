using GalaxyTaxi.Shared.Api.Models.JourneyGenerator;
using GalaxyTaxi.Shared.Api.Models.RouteGenerator;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace GalaxyTaxi.Shared.Api.Interfaces;

[Service("JourneyGenerator")]
public interface IJourneyGeneratorService
{
    Task<GenerateJourneysResponse> GenerateRoutesForCompany(GenerateJourneysRequest request, CallContext context = default);
}
