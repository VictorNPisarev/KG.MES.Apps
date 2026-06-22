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

	[JsonPropertyName("ready_date")]
	[Column("Готовность", Order = 1, DisplayFormat = "dd.MM.yyyy")]
	public DateTime? ReadyDate { get; set; }

	[JsonPropertyName("machine")]
	[Column("Станок", Order = 2, IsBadge = true)]
	public string? Machine { get; set; }

	[JsonPropertyName("lumber")]
	[Column("Пиломатериалы", Order = 3, IsBadge = true, DisplayGroup = "supply_status", CommentField = "LumberComment")]
	public string? Lumber { get; set; }

	[JsonPropertyName("lumber_comment")]
	public string? LumberComment { get; set; }

	[JsonPropertyName("paint")]
	[Column("ЛКМ", Order = 4, IsBadge = true, DisplayGroup = "supply_status", CommentField = "PaintComment")]
	public string? Paint { get; set; }

	[JsonPropertyName("paint_comment")]
	public string? PaintComment { get; set; }

	[JsonPropertyName("glass")]
	[Column("Стекло", Order = 5, IsBadge = true, DisplayGroup = "supply_status", CommentField = "GlassComment")]
	public string? Glass { get; set; }

	[JsonPropertyName("glass_comment")]
	public string? GlassComment { get; set; }

	[JsonPropertyName("furniture")]
	[Column("Фурнитура", Order = 6, IsBadge = true, DisplayGroup = "supply_status", CommentField = "FurnitureComment")]
	public string? Furniture { get; set; }

	[JsonPropertyName("furniture_comment")]
	public string? FurnitureComment { get; set; }

	[JsonPropertyName("alumwatershield")]
	[Column("ППС, В/О", Order = 7, IsBadge = true, DisplayGroup = "supply_status", CommentField = "AlumWaterShieldComment")]
	public string? AlumWaterShield { get; set; }

	[JsonPropertyName("alumWaterShield_comment")]
	public string? AlumWaterShieldComment { get; set; }
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