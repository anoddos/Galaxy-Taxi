using GalaxyTaxi.Shared.Api.Models.AccountSettings;
using Grpc.Core;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using GalaxyTaxi.Shared.Api.Models.VendorCompany;

namespace GalaxyTaxi.Web.Pages.Account;

public partial class Settings
{
	[Inject]
	public IDialogService DialogService { get; set; }

	private string oldPassword { get; set; } = "";
	private string newPassword { get; set; } = "";
	private string confirmPassword { get; set; } = "";
	private bool passwordsMatch => newPassword == confirmPassword;
	private IDialogReference dialogReference;


	private bool oldPasswordVisible = false;
	private bool newPasswordVisible = false;
	private bool confirmPasswordVisible = false;

	private string oldPasswordIcon = Icons.Material.Filled.Visibility;
	private string newPasswordIcon = Icons.Material.Filled.Visibility;
	private string confirmPasswordIcon = Icons.Material.Filled.Visibility;

	private InputType OldPasswordInput => oldPasswordVisible ? InputType.Text : InputType.Password;
	private InputType NewPasswordInput => newPasswordVisible ? InputType.Text : InputType.Password;
	private InputType ConfirmPasswordInput => confirmPasswordVisible ? InputType.Text : InputType.Password;

	private AccountSettings accountSettings { get; set; } = new AccountSettings();

	private bool showAlert = false;
	private string alertMessage = "";
	private Severity alertSeverity;



	private string OldPasswordValidation => IsAnyPasswordFilled && string.IsNullOrWhiteSpace(oldPassword)
		? "Old Password is required."
		: "";

	private string NewPasswordValidation => IsAnyPasswordFilled && string.IsNullOrWhiteSpace(newPassword)
		? "New Password is required."
		: "";

	private string ConfirmPasswordValidation => IsAnyPasswordFilled && string.IsNullOrWhiteSpace(confirmPassword)
		? "Confirm Password is required."
		: "";
	private IBrowserFile file;

	protected override async Task OnInitializedAsync()
	{
		var response = await _accountService.GetAccountSettings();
		if (response != null)
		{
			accountSettings = response;
		}
	}

	private void ToggleOldPasswordVisibility()
	{
		oldPasswordVisible = !oldPasswordVisible;
		oldPasswordIcon = oldPasswordVisible ? Icons.Material.Filled.VisibilityOff : Icons.Material.Filled.Visibility;
	}

	private void ToggleNewPasswordVisibility()
	{
		newPasswordVisible = !newPasswordVisible;
		newPasswordIcon = newPasswordVisible ? Icons.Material.Filled.VisibilityOff : Icons.Material.Filled.Visibility;
	}

	private void ToggleConfirmPasswordVisibility()
	{
		confirmPasswordVisible = !confirmPasswordVisible;
		confirmPasswordIcon = confirmPasswordVisible ? Icons.Material.Filled.VisibilityOff : Icons.Material.Filled.Visibility;
	}

	private async Task SaveChanges()
	{
		try
		{
			accountSettings = await _accountService.UpdateAccountSettings(new UpdateAccountSettingsRequest { NewPassword = newPassword, OldPassword = oldPassword, AccountInformation = accountSettings, Files = accountSettings.VendorFiles});
			
			alertMessage = "Account Settings Updated";
			alertSeverity = Severity.Success;
		}
		catch (RpcException ex)
		{
			alertMessage = ex.Status.Detail;
			alertSeverity = Severity.Error;
		}

		showAlert = true;
		StateHasChanged();
	}

	private void HandleAlertClosed()
	{
		showAlert = false;
		StateHasChanged();
	}

	private async Task OnUploadFiles(InputFileChangeEventArgs e)
	{
		foreach (var file in e.GetMultipleFiles())
		{
			var fileName = file.Name;
			var path = Path.Combine("C:", $"UploadedFiles/Vendors/{accountSettings.Email}", fileName);

			if (!Directory.Exists(Path.GetDirectoryName(path)))
			{
				Directory.CreateDirectory(Path.GetDirectoryName(path));
			}
			var vendorFile = new VendorFileModel 
			{ 
				Name = fileName,
				Path = path,
				Email = accountSettings.Email
			};
			try
			{
				accountSettings.VendorFiles.Add(vendorFile);
			}
			catch (RpcException ex)
			{
				//implement
			}
		}
	}
	private string PasswordMatchValidation() => newPassword == confirmPassword ? "" : "Passwords do not match";
	private void ValidatePasswords() => StateHasChanged();

	private bool IsAnyPasswordFilled => !string.IsNullOrWhiteSpace(oldPassword) || !string.IsNullOrWhiteSpace(newPassword) || !string.IsNullOrWhiteSpace(confirmPassword);
}