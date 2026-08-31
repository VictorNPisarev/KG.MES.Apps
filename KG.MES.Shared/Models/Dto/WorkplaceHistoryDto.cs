using System.Text.Json.Serialization;
using KG.MES.Shared.Attributes;

namespace KG.MES.Shared.Models.Dto;

public class WorkplaceHistoryDto
{
	[JsonPropertyName("operation_time")]
	[Column("Время", Order = 4, DisplayFormat = "dd.MM.yyyy HH:mm", Sortable = true)]
	public DateTime OperationTime { get; set; }

	[JsonPropertyName("operation_type")]
	[Column("Тип", Order = 0, IsBadge = true, BadgeGroup = "workplace_status", Sortable = true)]
	public string OperationType { get; set; } = string.Empty;

	[JsonPropertyName("order_number")]
	[Column("Заказ", Order = 2, Sortable = true)]
	public string OrderNumber { get; set; } = string.Empty;

	[JsonPropertyName("ready_date")]
	[Column("Готовность", Order = 3, DisplayFormat = "dd.MM.yyyy", Sortable = true)]
	public DateTime? ReadyDate { get; set; }

	[JsonPropertyName("user_name")]
	[Column("Сотрудник", Order = 5, Sortable = true)]
	public string? UserName { get; set; }

	[JsonPropertyName("notes")]
	[Column("Примечание", Order = 6)]
	public string? Notes { get; set; }

	[JsonPropertyName("window_count")]
	[Column("Окна, шт", Order = 7, ShowTotal = true)]
	public int WindowCount { get; set; }

	[JsonPropertyName("window_area")]
	[Column("Окна, м²", Order = 8, DisplayFormat = "F2", ShowTotal = true)]
	public decimal? WindowArea { get; set; }

	[JsonPropertyName("plate_count")]
	[Column("Щитовые, шт", Order = 9, ShowTotal = true)]
	public int PlateCount { get; set; }

	[JsonPropertyName("plate_area")]
	[Column("Щитовые, м²", Order = 10, DisplayFormat = "F2", ShowTotal = true)]
	public decimal? PlateArea { get; set; }

	[JsonPropertyName("is_econom")]
	[Column("Эконом", Order = 11, IsBadge = true)]
	public bool IsEconom { get; set; }

	[JsonPropertyName("is_claim")]
	[Column("Рекламация", Order = 12, IsBadge = true)]
	public bool IsClaim { get; set; }

	[JsonPropertyName("is_only_paid")]
	[Column("Оплачен, не запущен", Order = 13, IsBadge = true)]
	public bool IsOnlyPaid { get; set; }

	[JsonPropertyName("is_two_side_paint")]
	[Column("2-стор. покраска", Order = 14, IsBadge = true)]
	public bool IsTwoSidePaint { get; set; }

	[Column("***", Order = 1, Visible = false, IconConditions = new[] { "IsClaim:Claim",
																		"IsEconom:Econom",
																		"IsOnlyPaid:Paid",
																		"IsTwoSidePaint:TwoSidePaint",
																		"IsOnlyPlate:Plate" })]
	public string OrderFlags { get; } = string.Empty;

	public bool IsOnlyPlate
	{
		get
		{
			return WindowCount == 0 && PlateCount > 0;
		}
	}
}