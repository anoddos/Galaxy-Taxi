using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.AddressDetection;

[Serializable]
[ProtoContract]
public class DetectCoordinatesForCompanyEmployeesResponse
{
    [ProtoMember(1)] 
    public List<DetectAddressCoordinatesResponse> DetectResponses { get; set; } = null!;
}