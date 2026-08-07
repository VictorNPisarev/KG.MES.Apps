using KG.MES.Shared.Models.Dto;

namespace KG.MES.Shared.Services;

public class UserSessionService
{
	private LoginResponseDto? _loginResponse;

	public string? AccessToken => _loginResponse?.AccessToken;
	public string? RefreshToken => _loginResponse?.RefreshToken;
	public UserDto? User => _loginResponse?.User;
	public string? LicenseKey { get; private set; }
	public string? DeviceId { get; private set; }

	public bool IsAuthenticated => !string.IsNullOrEmpty(AccessToken) && User != null;

	public void SetSession(LoginResponseDto response, string licenseKey, string deviceId)
	{
		_loginResponse = response;
		LicenseKey = licenseKey;
		DeviceId = deviceId;
	}

	public void Clear()
	{
		_loginResponse = null;
		LicenseKey = null;
		DeviceId = null;
	}
}