using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.OfficeManagement
{
    [Serializable]
    [ProtoContract]
    public class GetOfficesResponse
    {
        [ProtoMember(1)]
        public List<OfficeInfo> Offices { get; set; }
    }
}
