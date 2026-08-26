using System.Text.Json.Serialization;
using KG.MES.Shared.Attributes;

namespace KG.MES.Shared.Models.Dto;

public class WorkplaceHistoryDto
{
	[JsonPropertyName("operation_time")]
	[Column("Время", Order = 3, DisplayFormat = "dd.MM.yyyy HH:mm")]
	public DateTime OperationTime { get; set; }

	[JsonPropertyName("operation_type")]
	[Column("Тип", Order = 0, IsBadge = true, BadgeGroup = "workplace_status")]
	public string OperationType { get; set; } = string.Empty;

	[JsonPropertyName("order_number")]
	[Column("Заказ", Order = 1)]
	public string OrderNumber { get; set; } = string.Empty;

	[JsonPropertyName("user_name")]
	[Column("Сотрудник", Order = 2)]
	public string? UserName { get; set; }

	[JsonPropertyName("notes")]
	[Column("Примечание", Order = 4)]
	public string? Notes { get; set; }

	[JsonPropertyName("window_count")]
	[Column("Окна, шт", Order = 5)]
	public int WindowCount { get; set; }

	[JsonPropertyName("window_area")]
	[Column("Окна, м²", Order = 6, DisplayFormat = "F2")]
	public decimal? WindowArea { get; set; }

	[JsonPropertyName("plate_count")]
	[Column("Щитовые, шт", Order = 7)]
	public int PlateCount { get; set; }

	[JsonPropertyName("plate_area")]
	[Column("Щитовые, м²", Order = 8, DisplayFormat = "F2")]
	public decimal? PlateArea { get; set; }
}