﻿using GalaxyTaxi.Shared.Api.Models.AddressDetection;
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
    public DateTime WorkingStartTime { get; set; }
    [ProtoMember(4)]
    public DateTime WorkingEndTime { get; set; }

}