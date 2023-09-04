using GalaxyTaxi.Shared.Api.Models.Common;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.AddressDetection;

[Serializable]
[ProtoContract]
public class DetectAddressCoordinatesResponse
{
    [ProtoMember(1)]
    public decimal Lat { get; set; }

    [ProtoMember(2)]
    public decimal Long { get; set; }
    
    [ProtoMember(3)]
    public AddressDetectionStatus StatusId { get; set; }
}