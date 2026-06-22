using KG.MES.Shared.Attributes;
using KG.MES.Shared.Interfaces;

namespace KG.MES.Shared.Models.ViewModels;

public class SupplyViewModel : IListItemViewModel
{
	[Column("№ заказа", Order = 0)]
	public string OrderNumber { get; set; } = string.Empty;

	[Column("Готовность", Order = 1, DisplayFormat = "dd.MM.yyyy")]
	public DateTime? ReadyDate { get; set; }

	[Column("Пиломатериалы", Order = 2, IsBadge = true, DisplayGroup = "supply_status", CommentField = "LumberComment")]
	public string? Lumber { get; set; }

	public string? LumberComment { get; set; }

	[Column("ЛКМ", Order = 3, IsBadge = true, DisplayGroup = "supply_status", CommentField = "PaintComment")]
	public string? Paint { get; set; }

	public string? PaintComment { get; set; }

	[Column("Стекло", Order = 4, IsBadge = true, DisplayGroup = "supply_status", CommentField = "GlassComment")]
	public string? Glass { get; set; }

	public string? GlassComment { get; set; }

	[Column("Фурнитура", Order = 5, IsBadge = true, DisplayGroup = "supply_status", CommentField = "FurnitureComment")]
	public string? Furniture { get; set; }

	public string? FurnitureComment { get; set; }

	[Column("ППС, В/О", Order = 6, IsBadge = true, DisplayGroup = "supply_status", CommentField = "AlumWaterShieldComment")]
	public string? AlumWaterShield { get; set; }

	public string? AlumWaterShieldComment { get; set; }

	public string? Machine { get; set; }
}