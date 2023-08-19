using BlazorInputFile;
using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using OfficeOpenXml;
using System.Net;
using System.Net.Http;

namespace GalaxyTaxi.Web.Pages.Account
{
    public partial class EmployeesInfo
    {
        private List<EmployeeJourneyInfo> _employees = new();
        private bool _loaded;
        //private string _searchString;
        private bool _sortNameByLength;
        private List<string> _events = new();
        private bool _isImporting;
        private IBrowserFile file;
        public string EmployeeNameFilter { get; set; }
        public string OfficeFilter { get; set; }
        // custom sort by name length
        protected override async Task OnInitializedAsync()
        {
            //GetEmployeesResponse response =  await _employeeManagement.GetEmployees();
            //if (response != null)
            //{
            //    _employees = response.Employees;
            //}
            
            _loaded = true;
        }

        private void OnFileChange(IBrowserFile selectedFile)
        {
            file = selectedFile;
        }


        private async Task ImportFromExcel()
        {
            _isImporting = true; // Set a flag to indicate importing
            StateHasChanged();

            try
            {
                using (var stream = file.OpenReadStream())
                {
                    using (var package = new ExcelPackage())
                    {
                        await package.LoadAsync(stream);

                        var worksheet = package.Workbook.Worksheets[0];
                        List<SingleEmployeeInfo> importedData = new List<SingleEmployeeInfo>();

                        for (int row = worksheet.Dimension.Start.Row + 1; row <= worksheet.Dimension.End.Row; row++)
                        {

                            string firstName = worksheet.Cells[row, 1].GetValue<string>();
                            string lastName = worksheet.Cells[row, 2].GetValue<string>();
                            string Mobile = worksheet.Cells[row, 3].GetValue<string>();
                            long CustomerCompanyId = worksheet.Cells[row, 4].GetValue<long>();
                            long OfficeId = worksheet.Cells[row, 5].GetValue<long>();
                            string Address = worksheet.Cells[row, 6].GetValue<string>();

                            SingleEmployeeInfo employee = new SingleEmployeeInfo
                            {
                                FirstName = firstName,
                                LastName = lastName,
                                Mobile = Mobile,
                                CustomerCompanyId = CustomerCompanyId,
                                OfficeId = OfficeId,
                                Address = Address
                            };
                            Console.WriteLine(firstName);

                            importedData.Add(employee);
                        }
                        Console.WriteLine(importedData.Count);

                        var request = new AddEmployeesRequest
                        {
                            employeesInfo = importedData
                        };
                        await _employeeManagement.AddEmployees(request);

                        _isImporting = false; // Reset the flag
                        StateHasChanged();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                // Handle exception, if needed
            }
        }


        private Func<EmployeeJourneyInfo, object> _sortBy => x =>
        {
            if (_sortNameByLength)
                return x.FirstName.Length;
            else
                return x.FirstName;
        };
        // quick filter - filter gobally across multiple columns with the same input
        private Func<EmployeeJourneyInfo, bool> _quickFilter => x =>
        {
            //if (string.IsNullOrWhiteSpace(_searchString))
            //    return true;

            //if (x.Sign.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            //    return true;

            //if (x.Name.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            //    return true;

            //if ($"{x.Number} {x.Position} {x.Molar}".Contains(_searchString))
            //    return true;

            return false;
        };

        // events
        void RowClicked(DataGridRowClickEventArgs<EmployeeJourneyInfo> args)
        {
            //_events.Insert(0, $"Event = RowClick, Index = {args.RowIndex}, Data = {System.Text.Json.JsonSerializer.Serialize(args.Item)}");
        }

        void SelectedItemsChanged(HashSet<EmployeeJourneyInfo> items)
        {
            //_events.Insert(0, $"Event = SelectedItemsChanged, Data = {System.Text.Json.JsonSerializer.Serialize(items)}");
        }
    }
}
