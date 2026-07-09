using System.Text.Json.Serialization;
using KG.MES.Shared.Attributes;
using KG.MES.Shared.Models.ViewModels;

namespace KG.MES.Shared.Models.Dto;

public class SupplyDto
{
	[JsonPropertyName("id")]
	public Guid Id { get; set; }

	[JsonPropertyName("order_number")]
	[Column("№ заказа", Order = 0)]
	public string OrderNumber { get; set; } = string.Empty;

	[JsonPropertyName("rtm_date")]
	[Column("Дата запуска", Visible = false, DisplayFormat = "dd.MM.yyyy")]
	public DateTime? RtmDate { get; set; }

	[JsonPropertyName("ready_date")]
	[Column("Готовность", Order = 1, DisplayFormat = "dd.MM.yyyy")]
	public DateTime? ReadyDate { get; set; }

	[JsonPropertyName("machine")]
	[Column("Станок", Order = 2, IsBadge = true)]
	public string? Machine { get; set; }

	[JsonPropertyName("production_order_id")]
	public Guid ProductionOrderId { get; set; }

	[JsonPropertyName("current_workplace_id")]
	public Guid? CurrentWorkplaceId { get; set; }

	[JsonPropertyName("current_status")]
	[Column("Статус", Order = 3, IsBadge = true, DisplayGroup = "workplace_name")]
	public string? Status { get; set; }

	[JsonPropertyName("lumber")]
	[Column("Пиломатериалы", Order = 4, IsBadge = true, DisplayGroup = "supply_status", CommentField = "LumberComment")]
	public string? Lumber { get; set; }

	[JsonPropertyName("lumber_comment")]
	[Column("Брус прим.", Order = 5, Visible = false, DisplayGroup = "supply_status")]
	public string? LumberComment { get; set; }

	[JsonPropertyName("paint")]
	[Column("ЛКМ", Order = 6, IsBadge = true, DisplayGroup = "supply_status", CommentField = "PaintComment")]
	public string? Paint { get; set; }

	[JsonPropertyName("paint_comment")]
	[Column("ЛКМ прим.", Order = 7, Visible = false, DisplayGroup = "supply_status")]
	public string? PaintComment { get; set; }

	[JsonPropertyName("glass")]
	[Column("Стекло", Order = 8, IsBadge = true, DisplayGroup = "supply_status", CommentField = "GlassComment")]
	public string? Glass { get; set; }

	[JsonPropertyName("glass_comment")]
	[Column("Стекло прим.", Order = 9, Visible = false, DisplayGroup = "supply_status")]
	public string? GlassComment { get; set; }

	[JsonPropertyName("furniture")]
	[Column("Фурнитура", Order = 10, IsBadge = true, DisplayGroup = "supply_status", CommentField = "FurnitureComment")]
	public string? Furniture { get; set; }

	[JsonPropertyName("furniture_comment")]
	[Column("Фурнитура прим.", Order = 11, Visible = false, DisplayGroup = "supply_status")]
	public string? FurnitureComment { get; set; }

	[JsonPropertyName("alumwatershield")]
	[Column("ППС, В/О", Order = 12, IsBadge = true, DisplayGroup = "supply_status", CommentField = "AlumWaterShieldComment")]
	public string? AlumWaterShield { get; set; }

	[JsonPropertyName("alumWaterShield_comment")]
	[Column("ППС, В/О прим.", Order = 13, Visible = false, DisplayGroup = "supply_status")]
	public string? AlumWaterShieldComment { get; set; }

	[JsonPropertyName("windowsill")]
	[Column("Отлив", Order = 14, IsBadge = true, DisplayGroup = "supply_status", CommentField = "WindowsillComment")]
	public string? Windowsill { get; set; }

	[JsonPropertyName("windowsill_comment")]
	[Column("Отлив прим.", Order = 15, Visible = false, DisplayGroup = "supply_status")]
	public string? WindowsillComment { get; set; }
}

public static class SupplyDtoExtension
{
	public static SupplyViewModel ToViewModel(this SupplyDto supplyDto)
	{
		return new SupplyViewModel
		{
			OrderNumber = supplyDto.OrderNumber,
			ReadyDate = supplyDto.ReadyDate,
			Lumber = supplyDto.Lumber,
			LumberComment = supplyDto.LumberComment,
			Paint = supplyDto.Paint,
			PaintComment = supplyDto.PaintComment,
			Glass = supplyDto.Glass,
			GlassComment = supplyDto.GlassComment,
			Furniture = supplyDto.Furniture,
			FurnitureComment = supplyDto.FurnitureComment,
			AlumWaterShield = supplyDto.AlumWaterShield,
			AlumWaterShieldComment = supplyDto.AlumWaterShieldComment,
			Machine = supplyDto.Machine
		};
	}

	public static List<SupplyViewModel> ToViewModels(this IEnumerable<SupplyDto> dtos)
		=> [.. dtos.Select(ToViewModel)];
}