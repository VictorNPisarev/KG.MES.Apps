using KG.MES.Shared.Constants;
using KG.MES.Shared.Models.Config;
using KG.MES.Shared.Models.Dto;
using KG.MES.Shared.Services;
using Microsoft.AspNetCore.Components;

namespace KG.MES.UI.Shared.Components;

public partial class WorkplaceStats
{
	[Inject] private ProductionApiService ApiService {get; set;} = null!;
	[Inject] private OrderViewSettings AppSettings { get; set; } = null!;

	private List<WorkplaceDto> workplaces = [];
	private WorkplaceStatsDto? stats;
	private List<BlockedOrderDto> blocks = [];
	private List<WorkplaceHistoryDto> history = [];
	private Guid? selectedWorkplaceId;
	private WorkplaceDto? selectedWorkplace;
	private DateTime dateFrom = DateTime.Now.AddDays(-7);
	private DateTime dateTo = DateTime.Now;
	private bool showDateFilter;
	private string activeTab = "orders";
	private List<OrderWorkplaceDto> workplaceOrders = [];
	private string orderFilter = "all";
	private DateTime orderDateFrom = DateTime.Now.AddDays(-7);
	private DateTime orderDateTo = DateTime.Now;
	private bool showOrderDateFilter;

	protected override async Task OnInitializedAsync()
	{
		Console.WriteLine("WorkplaceStats OnInitializedAsync");
		workplaces = await ApiService.GetWorkplacesAsync();//("active");
	}

	private async Task SelectWorkplace(WorkplaceDto wp)
	{
		selectedWorkplace = wp;
		activeTab = selectedWorkplace.IsWorkplace ? activeTab : "history";
		activeTab = selectedWorkplace.Code == WorkplaceCodes.None ? "orders" : activeTab;
		await SelectWorkplace(wp.Id);

	}

	private async Task SelectWorkplace(Guid id)
	{
		selectedWorkplaceId = id;
		stats = await ApiService.GetWorkplaceStatsAsync(id);
		blocks = await ApiService.GetWorkplaceBlocksAsync(id);
		history = await ApiService.GetWorkplaceHistoryAsync(id, dateFrom, dateTo, 1000);
		workplaceOrders = await ApiService.GetActiveAndPendingOrdersAsync(id);
		StateHasChanged();
	}

	private void ClearSelection()
	{
		selectedWorkplaceId = null;
		stats = null;
		blocks.Clear();
		history.Clear();
		workplaceOrders.Clear();
		StateHasChanged();
	}

	private async Task ApplyFilter()
	{
		showDateFilter = false;

		if (selectedWorkplaceId == null) return;
		
		history = await ApiService.GetWorkplaceHistoryAsync(
			(Guid)selectedWorkplaceId, dateFrom, dateTo, 1000);
		StateHasChanged();
	}

	private OrderViewSettings GetWorkplaceOrderSettings() => new()
	{
		ListEndpoint = $"orders/workplaces/{selectedWorkplaceId}/in-work",
		CardEndpoint = AppSettings.CardEndpoint,
		Title = "Заказы участка",
		ShowActions = false,
		CanEdit = false,
		CanExport = false,
		CanDelete = false,
		ShowTrace = false,
		ShowSupply = false,
		EditSupply = false
	};

	private List<OrderWorkplaceDto> filteredOrders => orderFilter == "all"
		? workplaceOrders
			.Where(o => o.ReadyDate >= orderDateFrom && o.ReadyDate <= orderDateTo.AddDays(1))
			.OrderBy(w => w.ReadyDate)
			.ToList()
		: workplaceOrders
			.Where(o => o.Status.ToLower() == orderFilter
						&& o.ReadyDate >= orderDateFrom
						&& o.ReadyDate <= orderDateTo.AddDays(1))
			.OrderBy(w => w.ReadyDate)
			.ToList();

	// Итоги по заказам
	private int TotalWindows => filteredOrders.Sum(o => o.WindowCount);
	private decimal TotalWindowArea => filteredOrders.Sum(o => o.WindowArea ?? 0);
	private int TotalPlates => filteredOrders.Sum(o => o.PlateCount);
	private decimal TotalPlateArea => filteredOrders.Sum(o => o.PlateArea ?? 0);
}