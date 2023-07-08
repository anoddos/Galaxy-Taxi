using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.AddressDetection;


[Serializable]
[ProtoContract]
public class DetectAddressCoordinatesRequest
{
    [ProtoMember(1)]
    public string Address { get; set; } = null!;
}