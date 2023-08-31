using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.Configuration;
using System.Net;

namespace GalaxyTaxi.Shared.Api.Interfaces;

[Service("AddressDetection")]
public interface IAddressDetectionService
{
    Task<DetectAddressCoordinatesResponse> DetectSingleAddressCoordinates(DetectAddressCoordinatesRequest request, CallContext context = default);
    Task<DetectCoordinatesForCompanyEmployeesResponse> DetectCoordinatesForCompanyEmployees(DetectCoordinatesForCompanyEmployeesRequest request, CallContext context = default);
    Task<AddressInfo> DetectAddressNameFromCoordinates(AddressInfo detectAddress);
    Task<AddressInfo> DetectAddressCoordinatesFromName(AddressInfo detectAddress);
}