using Microsoft.AspNetCore.Components;
using KG.MES.Shared.Models.Config;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using KG.MES.Shared.Models.Dto;
using KG.MES.Shared.Services;
using KG.MES.Shared.Helpers;
using KG.MES.UI.Shared.Components;

namespace KG.MES.UI.Shared.Components;
public partial class OrderListView<TOrder> : ComponentBase
{
	[Parameter] public OrderViewSettings Settings { get; set; } = new();
	[Parameter] public EventCallback<TOrder> OnOrderClick { get; set; }
	[Parameter] public RenderFragment? HeaderActions { get; set; }

	[Inject] private ProductionApiService ApiService { get; set; } = null!;
	[Inject] private IJSRuntime JSRuntime { get; set; } = null!;

	private TOrder? order;
	private OrderDashboard<TOrder>? dashboardRef;
	private string Endpoint => Settings.ListEndpoint;
	private string CardEndpoint => Settings.CardEndpoint;
	private string Title => Settings.Title;
	private bool ShowActions => Settings.ShowActions;
	private PaginatedResponse<TOrder> orders = new();
	private List<WorkplaceDto> workplaces = [];
	private List<ColumnInfo> columnInfos = [];
	private List<ColumnSetting> columnSettings = [];
	private bool isLoading = false;
	private string searchNumber = "";
	private int currentPage = 1;
	private int pageSize = 50;
	private string sortBy = "ready_date";
	private string sortOrder = "asc";
	private Guid? selectedWorkplaceId;
	private string tableKey => $"{Endpoint}_{typeof(TOrder).Name}";
	private bool isColumnsOpen = false;
	private bool useSplitView;
	private string savedPanelWidth = "66%";
	private DotNetObjectReference<OrderListView<TOrder>>? panelResizeRef;
	private string? lastReportedWidth;
	private bool _panelResizeInitialized = false;
	//private List<Guid> selectedWorkplaceIds = [];
	private bool dropdownOpen;
	private Guid[] selectedWorkplaceIds = [];

	protected override async Task OnInitializedAsync()
	{
		columnInfos = ColumnHelper.GetColumns<TOrder>();

		await LoadSettings();
		workplaces = await ApiService.GetAllWorkplacesAsync();//await ApiService.GetActiveWorkplacesAsync();
		await LoadOrders();
	}

	private async Task LoadSettings()
	{
		try
		{
			var splitViewSetting = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "useSplitView");
			useSplitView = splitViewSetting != null && splitViewSetting.ToLower() == "true";

			var splitPanelWidth = await JSRuntime.InvokeAsync<string>("localStorage.getItem", "splitPanelWidth");
			//if (!string.IsNullOrEmpty(splitPanelWidth))
			//	savedPanelWidth = splitPanelWidth;

			var json = await JSRuntime.InvokeAsync<string>("localStorage.getItem", $"table_settings_{tableKey}");
			columnSettings = TableSettingsManager.GetSettings<TOrder>(json);
		}
		catch
		{
			columnSettings = TableSettingsManager.GetDefaultSettings<TOrder>();
		}
	}

	private List<(ColumnInfo Info, ColumnSetting Setting)> BuildVisibleColumns()
	{
		return columnSettings
			.Where(s => s.Visible)
			.Join(columnInfos,
				s => s.PropertyName,
				i => i.PropertyName,
				(s, i) => (Info: i, Setting: s))
			.OrderBy(x => x.Setting.Order)
			.ToList();
	}

	private async Task SaveSettings()
	{
		var json = TableSettingsManager.Serialize(columnSettings);
		await JSRuntime.InvokeVoidAsync("localStorage.setItem", $"table_settings_{tableKey}", json);
	}

	private async Task ToggleColumn(string propertyName, bool visible)
	{
		var setting = columnSettings.FirstOrDefault(s => s.PropertyName == propertyName);
		if (setting != null)
		{
			setting.Visible = visible;
			await SaveSettings();
			StateHasChanged();
		}
	}

	private async Task MoveColumn(string propertyName, int direction)
	{
		var setting = columnSettings.FirstOrDefault(s => s.PropertyName == propertyName);
		if (setting == null) return;

		var orderedList = columnSettings.OrderBy(s => s.Order).ToList();
		var currentIndex = orderedList.IndexOf(setting);
		var newIndex = currentIndex + direction;

		if (newIndex < 0 || newIndex >= orderedList.Count) return;

		// Меняем местами
		var swap = orderedList[newIndex];
		var tempOrder = setting.Order;
		setting.Order = swap.Order;
		swap.Order = tempOrder;

		// Нормализуем порядок
		orderedList = columnSettings.OrderBy(s => s.Order).ToList();
		for (int i = 0; i < orderedList.Count; i++)
			orderedList[i].Order = i;

		await SaveSettings();
		StateHasChanged();
	}

	private async Task ResetColumns()
	{
		columnSettings = TableSettingsManager.GetDefaultSettings<TOrder>();
		await SaveSettings();
		StateHasChanged();
	}

	private async Task LoadOrders()
	{
		isLoading = true;
		StateHasChanged();

		try
		{
			orders = await ApiService.GetOrdersAsync<TOrder>(
				endpoint: Endpoint,
				workplaceId: selectedWorkplaceId,
				workplaceIds: selectedWorkplaceIds,
				orderNumber: string.IsNullOrEmpty(searchNumber) ? null : searchNumber,
				page: currentPage,
				limit: pageSize,
				sortBy: sortBy,
				sortOrder: sortOrder
			);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error loading orders: {ex.Message}");
		}
		finally
		{
			isLoading = false;
			StateHasChanged();
		}
	}

	private async Task ApplyFilters()
	{
		currentPage = 1;
		await LoadOrders();
	}

	private async Task GoToPage(int page)
	{
		currentPage = page;
		await LoadOrders();
	}

	private async Task OnPageSizeChanged()
	{
		currentPage = 1;
		await LoadOrders();
	}

	private async Task OnSearchKeyUp(KeyboardEventArgs e)
	{
		if (e.Key == "Enter")
			await ApplyFilters();
	}

	private List<int> GetPageNumbers()
	{
		var pages = new List<int>();
		var start = Math.Max(1, orders.Page - 2);
		var end = Math.Min(orders.TotalPages, orders.Page + 2);

		for (int i = start; i <= end; i++)
			pages.Add(i);

		return pages;
	}

	private TOrder? selectedOrder;

	private bool isModalOpen;

	private async Task OpenOrder(TOrder order)
	{
		selectedOrder = order;
		if (useSplitView)
		{
			// Просто обновляем панель — Dashboard сам переинициализируется
			StateHasChanged();
		}
		else
		{
			isModalOpen = true;
			StateHasChanged();
		}
	}

	private async Task CloseOrder()
	{
		selectedOrder = default;
		isModalOpen = false;

		if (useSplitView)
		{
			// Без перезагрузки списка — просто скрываем панель
			dashboardRef = null;
			StateHasChanged();
		}
		else
		{
			await LoadOrders();
			StateHasChanged();
		}
	}

	private Guid GetOrderId(TOrder order)
	{
		var prop = typeof(TOrder).GetProperty("Id");
		return (Guid)(prop?.GetValue(order) ?? Guid.Empty);
	}

	private string GetOrderNumber(TOrder order)
	{
		var prop = typeof(TOrder).GetProperty("OrderNumber");
		return prop?.GetValue(order)?.ToString() ?? "—";
	}

	private async Task ToggleViewMode()
	{
		useSplitView = !useSplitView;
		await JSRuntime.InvokeVoidAsync("localStorage.setItem", "useSplitView", useSplitView.ToString());

		StateHasChanged();
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (useSplitView && !_panelResizeInitialized)
		{
			//await InitializePanelResize(); //TODO доработать сохранение измененной ширины таблицы (панель в этом месте еще не существует)
		}
	}

	[JSInvokable]
	public async Task OnPanelResized(double width)
	{
		var newWidth = $"{width}px";

		// Защита от спама одинаковых значений (ResizeObserver может вызывать callback очень часто)
		if (newWidth == lastReportedWidth) return;
		lastReportedWidth = newWidth;

		await InvokeAsync(async () =>
		{
			savedPanelWidth = newWidth;
			try
			{
				await JSRuntime.InvokeVoidAsync("localStorage.setItem", "splitPanelWidth", savedPanelWidth);
			}
			catch {}

			StateHasChanged();
		});
	}

	private async Task InitializePanelResize()
	{
		if (_panelResizeInitialized) return;

		try
		{
			panelResizeRef = DotNetObjectReference.Create(this);
			await JSRuntime.InvokeVoidAsync("panelResize.init", panelResizeRef, "split-panel");
			_panelResizeInitialized = true;
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Failed to initialize panel resize: {ex.Message}");
			_panelResizeInitialized = false;
		}
	}

	private void ToggleDropdown()
	{
		dropdownOpen = !dropdownOpen;
	}

	private void CloseDropdown()
	{
		dropdownOpen = false;
	}

	private async Task ToggleWorkplace(Guid id, bool isChecked)
	{
		if (isChecked)
		{
			if (!selectedWorkplaceIds.Contains(id))
				selectedWorkplaceIds = selectedWorkplaceIds.Append(id).ToArray();
		}
		else
		{
			selectedWorkplaceIds = selectedWorkplaceIds.Where(x => x != id).ToArray();
		}

		//await ApplyFilters();
	}

	public void Dispose()
	{
		try
		{
			if (_panelResizeInitialized)
			{
				_ = JSRuntime.InvokeVoidAsync("panelResize.dispose", "split-panel");
			}
		}
		catch { }

		panelResizeRef?.Dispose();
	}
}