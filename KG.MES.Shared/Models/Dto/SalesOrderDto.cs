using System.Text.Json.Serialization;
using KG.MES.Shared.Attributes;

namespace KG.MES.Shared.Models.Dto;

public class SalesOrderDto
{
	[JsonPropertyName("id")]
	public Guid Id { get; set; }

	[JsonPropertyName("order_number")]
	[Column("№ заказа", Order = 1, IconConditions = new[] { "IsClaim:Claim", "IsEconom:Econom" })]
	public string OrderNumber { get; set; } = string.Empty;

	[JsonPropertyName("ready_date")]
	[Column("Дата готовности", Order = 2, DisplayFormat = "dd.MM.yyyy")]
	public DateTime? ReadyDate { get; set; }

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

	[JsonPropertyName("is_econom")]
	[Column("Эконом", Order = 10, IsBadge = true)]
	public bool IsEconom { get; set; }

	[JsonPropertyName("is_claim")]
	[Column("Рекламация", Order = 11, IsBadge = true)]
	public bool IsClaim { get; set; }

	[JsonPropertyName("is_only_paid")]
	[Column("Оплачен, не запущен", Order = 12, IsBadge = true)]
	public bool IsOnlyPaid { get; set; }

	[JsonPropertyName("created_at")]
	[Column("Дата запуска", Visible = false, DisplayFormat = "dd.MM.yyyy")]
	public DateTime StartDate { get; set; }

	[JsonPropertyName("production_order_id")]
	public Guid? ProductionOrderId { get; set; }

	[JsonPropertyName("current_workplace_id")]
	public Guid? CurrentWorkplaceId { get; set; }

	[JsonPropertyName("current_status")]
	[Column("Статус", Order = 3, IsBadge = true, DisplayGroup = "workplace_name")]
	public string? CurrentStatus { get; set; }

	[JsonPropertyName("manager_id")]
	public Guid? ManagerId { get; set; }

	[JsonPropertyName("manager_name")]
	[Column("Менеджер", Order = 4)]
	public string? ManagerName { get; set; }

	[JsonPropertyName("customer_id")]
	public Guid? CustomerId { get; set; }

	[JsonPropertyName("customer_name")]
	[Column("Контрагент", Order = 5)]
	public string? CustomerName { get; set; }

	[JsonPropertyName("amount")]
	[Column("Стоимость", Order = 6)]
	public decimal? Amount { get; set; }

	[JsonPropertyName("currency")]
	public string? Currency { get; set; }

	[Column("***", Order = 2, Visible = false, IconConditions = new[] { "IsClaim:Claim",
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