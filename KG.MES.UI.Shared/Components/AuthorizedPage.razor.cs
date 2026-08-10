using System.Text.Json;
using System.Text.Json.Serialization;
using KG.MES.Shared.Models.Dto;
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

	#region отладочные данные
	private DebugInfo? _debugInfo;
	private int _remainingSeconds;
	private Timer? _timer;
	private class DebugInfo
	{
		public string AccessToken { get; set; } = "";
		public string RefreshToken { get; set; } = "";
		public string LicenseKey { get; set; } = "";
		public string DeviceId { get; set; } = "";
		public DateTime ExpiresAt { get; set; }
	}

	private async Task UpdateTimer()
	{
		if (_debugInfo == null) return;
		_remainingSeconds = (int)(_debugInfo.ExpiresAt - DateTime.UtcNow).TotalSeconds;
		if (_remainingSeconds <= 0) _remainingSeconds = 0;
		await InvokeAsync(StateHasChanged);
	}

	private async Task ClearSession()
	{
		await JSRuntime.InvokeVoidAsync("localStorage.removeItem", "session_data");
		await JSRuntime.InvokeVoidAsync("localStorage.removeItem", "license_key");
		await JSRuntime.InvokeVoidAsync("localStorage.removeItem", "refresh_token");
		Session.Clear();
		NavManager.NavigateTo(NavManager.BaseUri + "login", true);
	}

	public void Dispose()
	{
		_timer?.Dispose();
	}
	#endregion

	//protected override async Task OnInitializedAsync()
	//{
	//	var token = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "access_token");
	//	_isAuthorized = !string.IsNullOrEmpty(token);
	//}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		//if (!firstRender) return;

		//if (Session.IsAuthenticated)
		//{
		//	_isAuthorized = true;
		//	StateHasChanged();
		//	return;
		//}

		// Пробуем восстановить сессию из localStorage
		var sessionJson = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "session_data");
		if (!string.IsNullOrEmpty(sessionJson))
		{
			var data = JsonSerializer.Deserialize<SessionData>(sessionJson);

			_debugInfo = new DebugInfo
			{
				AccessToken = data?.AccessToken ?? "",
				RefreshToken = data?.RefreshToken ?? "",
				LicenseKey = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "license_key") ?? "",
				DeviceId = await LicenseService.GetDeviceIdAsync(),
				ExpiresAt = data?.ExpiresAt ?? DateTime.MinValue
			};

			_remainingSeconds = (int)(_debugInfo.ExpiresAt - DateTime.UtcNow).TotalSeconds;
			_timer = new Timer(async _ => await UpdateTimer(), null, 0, 1000);

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
				var deviceHardwareId = await LicenseService.GetDeviceIdAsync();
				var licenseFile = await LicenseService.LoadLicenseAsync();
				var licenseKey = licenseFile?.LicenseKey
					?? await JSRuntime.InvokeAsync<string>("localStorage.getItem", "license_key") ?? "";

				if (!string.IsNullOrEmpty(licenseKey))
				{
					var request = new RefreshRequestDto
					{
						RefreshToken = data.RefreshToken,
						LicenseKey = licenseKey,
						DeviceHardwareId = deviceHardwareId
					};


					var response = await AuthService.RefreshAsync(request);
					if (response != null)
					{
						//Session.SetSession(response, licenseKey, deviceId);

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
				StateHasChanged();
			}

			//await JSRuntime.InvokeVoidAsync("localStorage.removeItem", "session_data");
		}

		_isAuthorized = false;
		NavManager.NavigateTo($"{NavManager.BaseUri}login");
		StateHasChanged();
	}

	private class SessionData
	{
		[JsonPropertyName("accessToken")] 
		public string AccessToken { get; set; } = "";

		[JsonPropertyName("refreshToken")]
		public string RefreshToken { get; set; } = "";

		[JsonPropertyName("expiresAt")]
		public DateTime ExpiresAt { get; set; }
	}
}