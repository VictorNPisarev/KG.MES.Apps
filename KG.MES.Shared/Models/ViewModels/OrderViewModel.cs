using KG.MES.Shared.Attributes;
using KG.MES.Shared.Models.Dto;
using KG.MES.Shared.Interfaces;

namespace KG.MES.Shared.Models.ViewModels;

public class OrderViewModel : IListItemViewModel
{
	[Column("№ заказа", Order = 1)]
	public string OrderNumber { get; set; } = string.Empty;

	[Column("Статус", Order = 3, IsBadge = true, DisplayGroup = "workplace_name")]
	public string? Status { get; set; }

	[Column("Дата запуска", Order = 4, DisplayFormat = "dd.MM.yyyy")]
	public DateTime StartDate { get; set; }

	[Column("Готовность", Order = 5, DisplayFormat = "dd.MM.yyyy")]
	public DateTime? ReadyDate { get; set; }

	[Column("Окна, шт", Order = 6)]
	public int WindowCount { get; set; }

	[Column("Окна, м2", Order = 7, DisplayFormat = "F2")]
	public double? WindowArea { get; set; }

	[Column("Щитовые, шт", Order = 8)]
	public int PlateCount { get; set; }

	[Column("Щитовые, м2", Order = 9, DisplayFormat = "F2")]
	public double? PlateArea { get; set; }

	[Column("Эконом", Order = 10, IsBadge = true)]
	public bool IsEconom { get; set; }

	[Column("Рекламация", Order = 11, IsBadge = true)]
	public bool IsClaim { get; set; }

	[Column("Оплачен, не запущен", Order = 12, IsBadge = true)]
	public bool IsOnlyPaid { get; set; }

	[Column("Контрагент", Visible = false)]
	public string CustomerName { get; set; } = string.Empty;

	[Column("Станок", Visible = true)]
	public string? Machine { get; set; }
}