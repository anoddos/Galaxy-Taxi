using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using ProtoBuf.Grpc;

namespace GalaxyTaxi.Shared.Api.Interfaces;

public interface IAddressDetectorService
{
    Task DetectAddressCoordinates(DetectAddressCoordinatesRequest request, CallContext context = default);
}