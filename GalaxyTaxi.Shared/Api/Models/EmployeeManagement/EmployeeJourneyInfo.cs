using GalaxyTaxi.Shared.Api.Models.AddressDetection;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.EmployeeManagement
{
    [Serializable]
    [ProtoContract]
    public class EmployeeJourneyInfo
    {
        [ProtoMember(1)]
        public string FirstName { get; set; }
        [ProtoMember(2)]
        public string LastName { get; set; }
        [ProtoMember(3)]
        public EmployeeAddressInfo From { get; set; }
        [ProtoMember(4)]
        public EmployeeAddressInfo To { get; set; }
        [ProtoMember(5)]
        public string Mobile { get; set; }
    }
}
