using GalaxyTaxi.Shared.Api.Interfaces;
using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Api.Api;

public class AddressDetectorService : IAddressDetectorService
{
    public Task DetectAddressCoordinates(DetectAddressCoordinatesRequest request, CallContext context = default)
    {
        throw new NotImplementedException();
    }
}