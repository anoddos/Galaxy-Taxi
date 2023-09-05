using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.Admin;

[ProtoContract]
[Serializable]
public class VendorFileModel
{
    [ProtoMember(1)]
    public string Email { get; set; }

    [ProtoMember(2)]
    public string Name { get; set; }

    [ProtoMember(3)]
    public string Path { get; set; }

    [ProtoMember(4)]
    public DateTime UploadDate { get; set; }
}

