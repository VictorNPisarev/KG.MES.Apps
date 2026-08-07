using System.Text.Json;
using KG.MES.Shared.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
namespace KG.MES.UI.Shared.Components;
public partial class AuthorizedPage
{
	[Parameter] public RenderFragment? ChildContent { get; set; }

	[Inject] private IJSRuntime JSRuntime { get; set; } = null!;
	[Inject] private LicenseService LicenseService { get; set; } = null!;
	[Inject] private UserSessionService Session { get; set; } = null!;
	[Inject] private AuthService AuthService { get; set; } = null!;

	private bool? _isAuthorized;

	//protected override async Task OnInitializedAsync()
	//{
	//	var token = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "access_token");
	//	_isAuthorized = !string.IsNullOrEmpty(token);
	//}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (!firstRender) return;

		if (Session.IsAuthenticated)
		{
			_isAuthorized = true;
			StateHasChanged();
			return;
		}

		// Пробуем восстановить сессию из localStorage
		var sessionJson = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "session_data");
		if (!string.IsNullOrEmpty(sessionJson))
		{
			var data = JsonSerializer.Deserialize<SessionData>(sessionJson);
			if (data != null && !string.IsNullOrEmpty(data.RefreshToken))
			{
				// Проверяем срок access_token
				if (data.ExpiresAt > DateTime.UtcNow)
				{
					// Токен ещё жив — используем его
					_isAuthorized = true;
					StateHasChanged();
					return;
				}

				// Токен протух — пробуем refresh
				var deviceId = await LicenseService.GetDeviceIdAsync();
				var licenseFile = await LicenseService.LoadLicenseAsync();
				var licenseKey = licenseFile?.LicenseKey
					?? await JSRuntime.InvokeAsync<string>("localStorage.getItem", "license_key") ?? "";

				if (!string.IsNullOrEmpty(licenseKey))
				{
					var response = await AuthService.RefreshAsync(data.RefreshToken, deviceId, licenseKey);
					if (response != null)
					{
						Session.SetSession(response, licenseKey, deviceId);

						var newSessionData = JsonSerializer.Serialize(new
						{
							accessToken = response.AccessToken,
							refreshToken = response.RefreshToken,
							expiresAt = DateTime.UtcNow.AddSeconds(response.ExpiresIn)
						});
						await JSRuntime.InvokeVoidAsync("localStorage.setItem", "session_data", newSessionData);

						_isAuthorized = true;
						StateHasChanged();
						return;
					}
				}
			}

			await JSRuntime.InvokeVoidAsync("localStorage.removeItem", "session_data");
		}

		_isAuthorized = false;
		NavManager.NavigateTo($"{NavManager.BaseUri}login");
		StateHasChanged();
	}

	private class SessionData
	{
		public string AccessToken { get; set; } = "";
		public string RefreshToken { get; set; } = "";
		public DateTime ExpiresAt { get; set; }
	}
}