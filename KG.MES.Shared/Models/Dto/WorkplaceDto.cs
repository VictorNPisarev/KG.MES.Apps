using System.Text.Json.Serialization;

namespace KG.MES.Shared.Models.Dto;

public class WorkplaceDto
{
	[JsonPropertyName("id")]
	public Guid Id { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("is_workplace")]
	public bool IsWorkplace { get; set; }

	[JsonPropertyName("level")]
	public int Level { get; set; }

	[JsonPropertyName("window_count")]
	public int WindowCount { get; set; }

	[JsonPropertyName("window_area")]
	public decimal WindowArea { get; set; }

	[JsonPropertyName("plate_count")]
	public int PlateCount { get; set; }

	[JsonPropertyName("plate_area")]
	public decimal PlateArea { get; set; }
}