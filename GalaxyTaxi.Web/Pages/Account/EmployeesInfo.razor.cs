using GalaxyTaxi.Shared.Api.Models.Common;
using GalaxyTaxi.Shared.Api.Models.EmployeeManagement;
using GalaxyTaxi.Shared.Api.Models.Filters;
using GalaxyTaxi.Shared.Api.Models.OfficeManagement;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using OfficeOpenXml;
using Microsoft.JSInterop;

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

	private EmployeeManagementFilter _employeeFilter = new EmployeeManagementFilter { 
		EmployeeName = string.Empty,
		JourneyStatus = EmployeeJourneyStatus.All,
		SelectedOffice = null
	};

	// custom sort by name length
	protected override async Task OnInitializedAsync()
	{
		await ReloadEmployees();
		var officeResponse = await _officeManagement.GetOffices(new OfficeManagementFilter());
		
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

	private async Task EmployeeNameChanged(string employeeName)
	{
		EmployeeNameFilter = employeeName;
		await ReloadEmployees();
	}

	private async Task UpdateEmployeeDataBasedOnSelection(EmployeeJourneyStatus status)
	{
		_employeeFilter.JourneyStatus = status;
		await ReloadEmployees();
	}


	private async Task ReloadEmployees()
	{
		_employeeFilter.SelectedOffice = OfficeFilter;
		_employeeFilter.EmployeeName = EmployeeNameFilter;
		var response = await _employeeManagement.GetEmployees(_employeeFilter);
		if (response != null)
		{
			_employees = response.Employees;
		}
		StateHasChanged();
	}

	private async Task OfficeValueChanged(OfficeInfo currentOffice)
	{
		OfficeFilter = currentOffice;
		await ReloadEmployees();
	}

	private async void DeleteEmployee(EmployeeJourneyInfo employee)
	{
		await _employeeManagement.DeleteEmployee(new DeleteEmployeeRequest { EmployeeId = employee.EmployeeId });
		_employees.RemoveAll(x => x.EmployeeId == employee.EmployeeId);
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

	private void GenerateExcelFile()
	{
		using (var package = new ExcelPackage())
		{
			var worksheet = package.Workbook.Worksheets.Add("Employees");

			for (int i = 0; i < ExcelColumnNames.AllColumns.Count; i++)
			{
				worksheet.Cells[1, i + 1].Value = ExcelColumnNames.AllColumns[i];
			}

			for (int i = 0; i < _employees.Count; i++)
			{
				worksheet.Cells[i + 2, 1].Value = _employees[i].FirstName;
				worksheet.Cells[i + 2, 2].Value = _employees[i].LastName;
				worksheet.Cells[i + 2, 3].Value =  _employees[i].Mobile;
				worksheet.Cells[i + 2, 4].Value = _employees[i].To?.OfficeId;
				worksheet.Cells[i + 2, 5].Value = _employees[i].From?.Name;
			}

			js.InvokeVoidAsync("saveAsFile", $"Employees_{DateTime.Now.ToString("yyyy:MM:dd_HH:mm")}.xlsx", Convert.ToBase64String(package.GetAsByteArray()));
		}
	}


	private async Task ImportFromExcel()
	{
		_isImporting = true;

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
						var employee = new SingleEmployeeInfo();

						foreach (var column in ExcelColumnNames.AllColumns)
						{
							var columnIndex = worksheet.Cells[1, 1, 1, worksheet.Dimension.End.Column]
												.FirstOrDefault(cell => cell.Text == column)?.Start.Column ?? 0;
							var cellValue = worksheet.Cells[row, columnIndex].Text;

							switch (column)
							{
								case ExcelColumnNames.FirstName:
									employee.FirstName = cellValue;
									break;
								case ExcelColumnNames.LastName:
									employee.LastName = cellValue;
									break;
								case ExcelColumnNames.Mobile:
									employee.Mobile = cellValue;
									break;
								case ExcelColumnNames.OfficeId:
									employee.OfficeId = long.Parse(cellValue);
									break;
								case ExcelColumnNames.Address:
									employee.Address = cellValue;
									break;
							}
						}

						importedData.Add(employee);
					}

					Console.WriteLine(importedData.Count);

					var request = new AddEmployeesRequest
					{
						EmployeesInfo = importedData
					};
					await _employeeManagement.AddEmployees(request);

					_isImporting = false; // Reset the flag
					ReloadEmployees();
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			// Handle exception, if needed
		}

	}

}