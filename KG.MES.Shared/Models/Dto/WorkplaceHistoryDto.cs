using System.Text.Json.Serialization;
using KG.MES.Shared.Attributes;

namespace KG.MES.Shared.Models.Dto;
public class WorkplaceHistoryDto
{
	[JsonPropertyName("operation_time")]
	public DateTime OperationTime { get; set; }

	[JsonPropertyName("operation_type")]
	[Column("Операция", IsBadge = true)]
	public string OperationType { get; set; } = string.Empty;

	[JsonPropertyName("order_number")]
	public string OrderNumber { get; set; } = string.Empty;

	[JsonPropertyName("window_count")]
	[Column("Окна, шт", Order = 6)]
	public int WindowCount { get; set; }

	[JsonPropertyName("window_area")]
	[Column("Окна, м2", Order = 7, DisplayFormat = "F2")]
	public double? WindowArea { get; set; }

	[JsonPropertyName("plate_count")]
	[Column("Щитовые, шт", Order = 8)]
	public int PlateCount { get; set; }

	[JsonPropertyName("plate_area")]
	[Column("Щитовые, м2", Order = 9, DisplayFormat = "F2")]
	public double? PlateArea { get; set; }

	[JsonPropertyName("user_name")]
	public string? UserName { get; set; }

	[JsonPropertyName("notes")]
	public string? Notes { get; set; }
}
