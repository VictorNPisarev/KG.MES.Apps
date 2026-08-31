using System.Text.Json.Serialization;
using KG.MES.Shared.Attributes;

namespace KG.MES.Shared.Models.Dto;

public class OrderWorkplaceDto
{
	[JsonPropertyName("id")]
	public Guid ProductionOrderId { get; set; } // Это будет productionOrderId

	[JsonPropertyName("workplace_id")]
	public Guid WorkplaceId { get; set; }

	[JsonPropertyName("status")]
	[Column("Статус", Order = 3, IsBadge = true, DisplayGroup = "workplace_name", Sortable = true)]
	public string Status { get; set; } = string.Empty;

	[JsonPropertyName("order_id")]
	public Guid OrderId { get; set; }

	[JsonPropertyName("order_number")]
	[Column("№ заказа", Order = 1, IconConditions = new[] { "IsClaim:Claim", "IsEconom:Econom" }, Sortable = true)]
	public string OrderNumber { get; set; } = string.Empty;

	[JsonPropertyName("window_count")]
	[Column("Окна, шт", Order = 6, ShowTotal = true)]
	public int WindowCount { get; set; }

	[JsonPropertyName("window_area")]
	[Column("Окна, м2", Order = 7, DisplayFormat = "F2", ShowTotal = true)]
	public decimal? WindowArea { get; set; }

	[JsonPropertyName("plate_count")]
	[Column("Щитовые, шт", Order = 8, ShowTotal = true)]
	public int PlateCount { get; set; }

	[JsonPropertyName("plate_area")]
	[Column("Щитовые, м2", Order = 9, DisplayFormat = "F2", ShowTotal = true)]
	public decimal? PlateArea { get; set; }

	[JsonPropertyName("ready_date")]
	[Column("Готовность", Order = 5, DisplayFormat = "dd.MM.yyyy", Sortable = true)]
	public DateTime? ReadyDate { get; set; }

	[JsonPropertyName("is_econom")]
	[Column("Эконом", Order = 10, IsBadge = true)]
	public bool IsEconom { get; set; }

	[JsonPropertyName("is_claim")]
	[Column("Рекламация", Order = 11, IsBadge = true)]
	public bool IsClaim { get; set; }

	[JsonPropertyName("is_only_paid")]
	[Column("Оплачен, не запущен", Order = 12, IsBadge = true)]
	public bool IsOnlyPaid { get; set; }

	[JsonPropertyName("workplaceOrderStatus")]
	public string WorkplaceOrderStatus { get; set; } = string.Empty;

	[JsonPropertyName("fromJoinery")]
	public bool FromJoinery { get; set; }

	[JsonPropertyName("Name")]
	public string Name { get; set; } = string.Empty;

	[JsonPropertyName("isBlocked")]
	public bool IsBlocked { get; set; }

	[JsonPropertyName("blocks")]
	public List<OrderBlockDto> Blocks { get; set; } = new();

	[JsonPropertyName("attributes")]
	public List<OrderAttributeDto> Attributes { get; set; } = new();

}