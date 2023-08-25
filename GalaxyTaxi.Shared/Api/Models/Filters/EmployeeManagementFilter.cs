using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.Filters
{
    [Serializable]
    [ProtoContract]
    public class EmployeeManagementFilter
    {
        [ProtoMember(1)]
        public long CustomerCompanyId { get; set; } // todo es arasworia frontidan ar unda wavides
    }
}
