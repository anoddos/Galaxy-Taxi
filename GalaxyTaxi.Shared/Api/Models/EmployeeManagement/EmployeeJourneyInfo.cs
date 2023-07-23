using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.EmployeeManagement
{
    public class EmployeeJourneyInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string Mobile { get; set; }
    }
}
