using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using GalaxyTaxi.Shared.Api.Models.Filters;
using GalaxyTaxi.Shared.Api.Models.OfficeManagement;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using OfficeOpenXml;
using static MudBlazor.CategoryTypes;

namespace GalaxyTaxi.Web.Pages.Account;

public partial class EmployeesInfo
{
    private List<EmployeeJourneyInfo> _employees = new List<EmployeeJourneyInfo>();
    private List<OfficeInfo> _offices = new List<OfficeInfo>();
    private bool _loaded;
    //private string _searchString;
    private bool _sortNameByLength;
    private List<string> _events = new();
    private bool _isImporting;
    private IBrowserFile file;
    private bool _isOpen;
    //private MudDialog dialog;
    public string EmployeeNameFilter { get; set; }
    public OfficeInfo OfficeFilter { get; set; }

    private IDialogReference dialogReference;

    private EmployeeJourneyInfo selectedEmployeeForEdit;

    // custom sort by name length
    protected override async Task OnInitializedAsync()
    {
        var response = await _employeeManagement.GetEmployees(new EmployeeManagementFilter());
        var officeResponse = await _officeManagement.GetOffices(new OfficeManagementFilter());
        if (response != null && response.Employees != null)
        {
            _employees = response.Employees;
        }
        if (officeResponse != null && officeResponse.Offices != null)
        {
            _offices = officeResponse.Offices;
        }

        _loaded = true;
        _isOpen = false;
    }

    private void OnFileChange(IBrowserFile selectedFile)
    {
        file = selectedFile;
    }

    private async Task OfficeValueChanged(OfficeInfo currentOffice)
    {
        StateHasChanged();

        var response = await _employeeManagement.GetEmployees(new EmployeeManagementFilter { SelectedOffice = currentOffice });
        if (response != null)
        {
            _employees = response.Employees;
        }
        StateHasChanged();
    }

    private async void OpenEditPopup(EmployeeJourneyInfo employee)
    {
        selectedEmployeeForEdit = employee;
        var parameters = new DialogParameters { ["Employee"] = selectedEmployeeForEdit, ["OfficeList"] = _offices };
        dialogReference = DialogService.Show<EmployeePopup>("Edit Employee", parameters);
		var result = await dialogReference.Result;
        dialogReference.Close();
        StateHasChanged();
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
                    var importedData = new List<SingleEmployeeInfo>();

                    for (int row = worksheet.Dimension.Start.Row + 1; row <= worksheet.Dimension.End.Row; row++)
                    {

                        var firstName = worksheet.Cells[row, 1].GetValue<string>();
                        var lastName = worksheet.Cells[row, 2].GetValue<string>();
                        var mobile = worksheet.Cells[row, 3].GetValue<string>();
                        var customerCompanyId = worksheet.Cells[row, 4].GetValue<long>();
                        var officeId = worksheet.Cells[row, 5].GetValue<long>();
                        var address = worksheet.Cells[row, 6].GetValue<string>();

                        SingleEmployeeInfo employee = new SingleEmployeeInfo
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            Mobile = mobile,
                            CustomerCompanyId = customerCompanyId,
                            OfficeId = officeId,
                            Address = address
                        };
                        Console.WriteLine(firstName);

                        importedData.Add(employee);
                    }
                    Console.WriteLine(importedData.Count);

                    var request = new AddEmployeesRequest
                    {
                        EmployeesInfo = importedData
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


    private Func<EmployeeJourneyInfo, object> SortBy => x =>
    {
        if (_sortNameByLength)
            return x.FirstName.Length;
        else
            return x.FirstName;
    };
    // quick filter - filter gobally across multiple columns with the same input
    private Func<EmployeeJourneyInfo, bool> QuickFilter => x =>
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