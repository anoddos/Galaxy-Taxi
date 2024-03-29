using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.AddressDetection;

[Serializable]
[ProtoContract]
public class DetectAddressCoordinatesRequest
{
    [ProtoMember(1)]
    public long EmployeeId { get; set; }
}