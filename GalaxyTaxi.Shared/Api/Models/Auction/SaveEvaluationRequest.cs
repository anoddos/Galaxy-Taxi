using GalaxyTaxi.Shared.Api.Models.Common;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.Auction;

[ProtoContract]
[Serializable]
public class SaveEvaluationRequest
{
    [ProtoMember(1)] 
    public long Id { get; set; }
    
    [ProtoMember(2)] 
    public Feedback Evaluation { get; set; }

    [ProtoMember(3)] 
    public string Comment { get; set; } = null!;
}