using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.AddressDetection
{
    [Serializable]
    [ProtoContract]
    public class AddressInfo
    {
        [ProtoMember(1)]
        public string Name { get; set; } = null!;
        
        [ProtoMember(2)]
        public decimal? Latitude { get; set; }
        
        [ProtoMember(3)]
        public decimal? Longitude { get; set; }
        
        [ProtoMember(4)]
        public long Id { get; set; }
    }
}
