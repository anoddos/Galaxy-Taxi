using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Payment;

[ProtoContract]
[Serializable]
public class PaymentResponse
{
    [ProtoMember(1)]
    public string PaymentStatus { get; set; }
}