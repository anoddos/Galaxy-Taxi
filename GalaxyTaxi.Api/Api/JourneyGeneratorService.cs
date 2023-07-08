using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.RouteGenerator;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Api.Api;

public class JourneyGeneratorService : IJourneyGeneratorService
{
    public Task<GenerateJourneysResponse> GenerateRoutesForCompany(GenerateJourneysRequest request, CallContext context = default)
    {
        throw new NotImplementedException();
    }
}