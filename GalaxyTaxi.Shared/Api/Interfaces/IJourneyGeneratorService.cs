using GalaxyTaxi.Shared.Api.Models.RouteGenerator;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Shared.Api.Interfaces;

public interface IJourneyGeneratorService
{
    Task<GenerateJourneysResponse> GenerateRoutesForCompany(GenerateJourneysRequest request, CallContext context = default);
}
