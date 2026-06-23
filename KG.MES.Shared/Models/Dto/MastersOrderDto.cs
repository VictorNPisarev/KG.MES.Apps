using System.Text.Json.Serialization;
using KG.MES.Shared.Attributes;
using KG.MES.Shared.Models.ViewModels;

namespace KG.MES.Shared.Models.Dto;

public class MastersOrderDto
{
	[JsonPropertyName("id")]
	public Guid Id { get; set; }

	[JsonPropertyName("order_number")]
	[Column("№ заказа", Order = 1)]
	public string OrderNumber { get; set; } = string.Empty;

	[JsonPropertyName("ready_date")]
	[Column("Готовность", Order = 2, DisplayFormat = "dd.MM.yyyy")]
	public DateTime? ReadyDate { get; set; }

	[JsonPropertyName("current_status")]
	[Column("Статус", Order = 3, IsBadge = true, DisplayGroup = "workplace_name")]
	public string? Status { get; set; }

	[JsonPropertyName("window_count")]
	[Column("Окна, шт", Order = 4)]
	public int WindowCount { get; set; }

	[JsonPropertyName("window_area")]
	[Column("Окна, м2", Order = 5, DisplayFormat = "F2")]
	public double? WindowArea { get; set; }

	[JsonPropertyName("plate_count")]
	[Column("Щитовые, шт", Order = 6)]
	public int PlateCount { get; set; }

	[JsonPropertyName("plate_area")]
	[Column("Щитовые, м2", Order = 7, DisplayFormat = "F2")]
	public double? PlateArea { get; set; }

	[Column("Станок", Order = 8, Visible = true, IsBadge = true)]
	public string? Machine { get; set; }

	[JsonPropertyName("created_at")]
	[Column("Дата запуска", Visible = false)]
	public DateTime StartDate { get; set; }

	[JsonPropertyName("is_econom")]
	[Column("Эконом", Visible = false, IsBadge = true)]
	public bool IsEconom { get; set; }

	[JsonPropertyName("is_claim")]
	[Column("Рекламация", Visible = false, IsBadge = true)]
	public bool IsClaim { get; set; }

	[JsonPropertyName("is_only_paid")]
	[Column("Оплачен, не запущен", Visible = false, IsBadge = true)]
	public bool IsOnlyPaid { get; set; }

	[JsonPropertyName("production_order_id")]
	public string? ProductionOrderId { get; set; }

	[JsonPropertyName("current_workplace_id")]
	public string? CurrentWorkplaceId { get; set; }

	[JsonPropertyName("customer_name")]
	[Column("Контрагент", Visible = false)]
	public string CustomerName { get; set; } = string.Empty;

	[JsonPropertyName("current_workplace_name")]
	[Column("current_workplace_name", Visible = false)]
	public string? CurrentWorkplaceName { get; set; }

}


public static class MastersOrderDtoExtension
{
	public static MastersOrderViewModel ToViewModel(this MastersOrderDto orderDto)
	{
		return new MastersOrderViewModel
		{
			OrderNumber = orderDto.OrderNumber,
			Status = orderDto.Status,
			StartDate = orderDto.StartDate,
			ReadyDate = orderDto.ReadyDate,
			WindowCount = orderDto.WindowCount,
			WindowArea = orderDto.WindowArea,
			PlateCount = orderDto.PlateCount,
			PlateArea = orderDto.PlateArea,
			IsEconom = orderDto.IsEconom,
			IsClaim = orderDto.IsClaim,
			IsOnlyPaid = orderDto.IsOnlyPaid,
			CustomerName = orderDto.CustomerName,
			Machine = orderDto.Machine
		};
	}

	public static List<MastersOrderViewModel> ToViewModels(this IEnumerable<MastersOrderDto> dtos)
		=> [.. dtos.Select(ToViewModel)];
}