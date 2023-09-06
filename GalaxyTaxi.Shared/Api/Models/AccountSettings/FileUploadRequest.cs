using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.AccountSettings;

[ProtoContract]
[Serializable]
public class FileUploadRequest
{
    [ProtoMember(1)]
    public byte[] content { get; set; }

    [ProtoMember(2)]
    public string FileName { get; set; }

    [ProtoMember(3)]
    public string VendorEmail { get; set; }
}
