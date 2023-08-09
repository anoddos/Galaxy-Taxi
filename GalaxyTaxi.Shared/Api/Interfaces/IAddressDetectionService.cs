using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;

namespace GalaxyTaxi.Shared.Api.Interfaces;

[Service("AddressDetection")]
public interface IAddressDetectionService
{
    Task<DetectAddressCoordinatesResponse> DetectAddressCoordinates(DetectAddressCoordinatesRequest request, CallContext context = default);
}