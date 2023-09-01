using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.OfficeManagement;

[Serializable]
[ProtoContract]
public class OfficeInfo
{
	[ProtoMember(1)]
	public long OfficeId { get; set; }

	[ProtoMember(2)]
	public AddressInfo Address { get; set; }

	[ProtoMember(3)]
	public TimeSpan WorkingStartTime { get; set; }

	[ProtoMember(4)]
	public TimeSpan WorkingEndTime { get; set; }

	[ProtoMember(5)]
	public int NumberOfEmployees { get; set; }
}