using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.JourneyGenerator;

[ProtoContract]
[Serializable]
public class GenerateJourneysResponse
{
    [ProtoMember(1)] 
    public List<JourneyInfo> Journeys { get; set; } = null!;
}