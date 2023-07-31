using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using Microsoft.AspNetCore.Authorization;
using System.Net;

namespace GalaxyTaxi.Web.Pages.Account
{
    public partial class EmployeesInfo
    {
        private List<EmployeeJourneyInfo> _employees = new();
        private bool _loaded;
        protected override async Task OnInitializedAsync()
        {
            GetEmployeesResponse response = await _employeeManagement.GetEmployees();
            if (response != null)
            {
                _employees = response.Employees;
            }
            
            _loaded = true;
        }
    }
}
