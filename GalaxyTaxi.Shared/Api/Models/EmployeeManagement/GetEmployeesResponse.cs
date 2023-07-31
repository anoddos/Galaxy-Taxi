using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalaxyTaxi.Shared.Api.Models.EmployeeManagement
{
    public class GetEmployeesResponse
    {
        public List<EmployeeJourneyInfo> Employees { get; set; }

        public static implicit operator List<object>(GetEmployeesResponse v)
        {
            throw new NotImplementedException();
        }
    }
}
