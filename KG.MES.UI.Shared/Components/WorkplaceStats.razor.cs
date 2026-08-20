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
	private DateTime dateFrom = DateTime.Now.AddDays(-7);
	private DateTime dateTo = DateTime.Now;
	private bool showDateFilter;
	private string activeTab = "orders";
	private List<OrderWorkplaceDto> workplaceOrders = [];
	private string orderFilter = "all";
	protected override async Task OnInitializedAsync()
	{
		Console.WriteLine("WorkplaceStats OnInitializedAsync");
		workplaces = await ApiService.GetWorkplacesAsync();//("active");
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
}