using KG.MES.Shared.Models.Dto;
using KG.MES.Shared.Services;
using KG.MES.Shared.Models.Enums;
using Microsoft.AspNetCore.Components;

namespace KG.MES.Main.Pages;

public partial class CreateOrderPage
{
	private string OrderNumber { get; set; } = "";
	private int WindowCount { get; set; }
	private double WindowArea { get; set; }
	private int PlateCount { get; set; }
	private double PlateArea { get; set; }
	private string Machine { get; set; } = "";
	private DateTime StartDate { get; set; } = DateTime.Now;
	private int ApprovedDays { get; set; }
	private int UnapprovedDays { get; set; }
	private DateTime? So8Date { get; set; }
	private string Comment { get; set; } = "";
	private bool IsEconom { get; set; }
	private bool IsClaim { get; set; }
	private bool IsOnlyPaid { get; set; }
	private bool IsTwoSidePaint { get; set; }

	[Inject] private ProductionApiService ApiService { get; set; } = null!;

	private DateTime? ReadyDate { get; set; }

	private bool isCalculating;
	private bool isSaving;
	private string StatusMessage { get; set; } = "";
	private bool isError;

	/// <summary>
	/// Вызывается при изменении даты начала или количества дней.
	/// </summary>
	private async Task OnParametersChanged()
	{
		await CalculateReadyDateAsync();
	}

	private async Task SaveOrder()
	{
		if (string.IsNullOrWhiteSpace(OrderNumber))
		{
			StatusMessage = "Введите номер заказа";
			isError = true;
			return;
		}

		isSaving = true;
		StatusMessage = "";

		try
		{
			var dto = new ProductionOrderExportDto
			{
				OrderNumber = OrderNumber,
				WindowCount = WindowCount,
				WindowArea = WindowArea,
				PlateCount = PlateCount,
				PlateArea = PlateArea,
				Comment = Comment,
				IsEconom = IsEconom,
				IsClaim = IsClaim,
				IsOnlyPaid = IsOnlyPaid,
				IsTwoSidePaint = IsTwoSidePaint,
				StartDate = StartDate,
				ProductionDays = ApprovedDays > 0 ? ApprovedDays : UnapprovedDays,
				ReadyDate = ReadyDate
			};

			var success = await ApiService.ExportToProductionAsync(dto);

			if (success)
			{
				StatusMessage = $"Заказ №{OrderNumber} создан";
				isError = false;
				// Очистка формы
				OrderNumber = "";
				WindowCount = 0;
				WindowArea = 0;
				PlateCount = 0;
				PlateArea = 0;
				Comment = "";
				IsEconom = false;
				IsClaim = false;
				IsOnlyPaid = false;
				ReadyDate = null;
				ApprovedDays = 0;
				UnapprovedDays = 0;
				So8Date = null;
			}
			else
			{
				StatusMessage = "Ошибка при создании заказа";
				isError = true;
			}
		}
		catch (Exception ex)
		{
			StatusMessage = $"Ошибка: {ex.Message}";
			isError = true;
		}
		finally
		{
			isSaving = false;
		}
	}

	/// <summary>
	/// Рассчитывает дату готовности через API с учетом производственного календаря.
	/// </summary>
	private async Task CalculateReadyDateAsync()
	{
		var days = ApprovedDays > 0 ? ApprovedDays : UnapprovedDays;

		if (days <= 0)
		{
			ReadyDate = null;
			return;
		}

		isCalculating = true;
		StateHasChanged(); // Обновляем UI, чтобы показать индикатор загрузки

		try
		{
			ReadyDate = await ApiService.CalculateReadyDateAsync(StartDate, days);
			StatusMessage = "";
			isError = false;
		}
		catch (Exception ex)
		{
			StatusMessage = $"Ошибка расчета даты: {ex.Message}";
			isError = true;
			ReadyDate = null;
		}
		finally
		{
			isCalculating = false;
			StateHasChanged();
		}
	}
}
