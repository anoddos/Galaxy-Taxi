﻿using ProtoBuf;

namespace GalaxyTaxi.Shared.Api.Models.AddressDetection;

[Serializable]
[ProtoContract]
public class AddressInfo
{
    [ProtoMember(1)]
    public string Name { get; set; } = null!;

    [ProtoMember(2)]
    public double? Latitude { get; set; }

    [ProtoMember(3)]
    public double? Longitude { get; set; }

    [ProtoMember(4)]
    public long Id { get; set; }

    [ProtoMember(5)]
    public bool IsDetected { get; set; }
}