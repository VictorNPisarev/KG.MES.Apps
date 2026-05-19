using System.Text.Json.Serialization;

namespace KG.MES.Shared.Helpers;

public class BadgeConfig
{
	[JsonPropertyName("groups")]
	public Dictionary<string, Dictionary<string, string>> Groups { get; set; } = [];

	[JsonPropertyName("defaults")]
	public Dictionary<string, string> Defaults { get; set; } = [];
}