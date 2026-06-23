using KG.MES.Shared.Attributes;
using KG.MES.Shared.Interfaces;

namespace KG.MES.Shared.Models.ViewModels;

public class MastersOrderViewModel : IListItemViewModel
{
	[Column("№ заказа", Order = 1)]
	public string OrderNumber { get; set; } = string.Empty;

	[Column("Готовность", Order = 2, DisplayFormat = "dd.MM.yyyy")]
	public DateTime? ReadyDate { get; set; }

	[Column("Статус", Order = 3, IsBadge = true, DisplayGroup = "workplace_name")]
	public string? Status { get; set; }

	[Column("Окна, шт", Order = 4)]
	public int WindowCount { get; set; }

	[Column("Окна, м2", Order = 5, DisplayFormat = "F2")]
	public double? WindowArea { get; set; }

	[Column("Щитовые, шт", Order = 6)]
	public int PlateCount { get; set; }

	[Column("Щитовые, м2", Order = 7, DisplayFormat = "F2")]
	public double? PlateArea { get; set; }

	[Column("Станок", Order = 8, Visible = true)]
	public string? Machine { get; set; }

	[Column("Дата запуска", Visible = false)]
	public DateTime StartDate { get; set; }

	[Column("Эконом", Visible = false, IsBadge = true)]
	public bool IsEconom { get; set; }

	[Column("Рекламация", Visible = false, IsBadge = true)]
	public bool IsClaim { get; set; }

	[Column("Оплачен, не запущен", Visible = false, IsBadge = true)]
	public bool IsOnlyPaid { get; set; }

	[Column("Контрагент", Visible = false)]
	public string CustomerName { get; set; } = string.Empty;
}