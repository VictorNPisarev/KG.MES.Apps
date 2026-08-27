using KG.MES.Shared.Helpers;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace KG.MES.UI.Shared.Components;
public partial class DynamicTable<TListItem> : ComponentBase
{
	[Parameter] public IEnumerable<TListItem> Items { get; set; } = [];
	[Parameter] public List<ColumnInfo> ColumnInfos { get; set; } = [];
	[Parameter] public List<ColumnSetting> ColumnSettings { get; set; } = [];
	[Parameter] public string TableKey { get; set; } = "dynamic-table";
	[Parameter] public bool ShowActions { get; set; }
	[Parameter] public RenderFragment? RowActions { get; set; }
	[Parameter] public RenderFragment<TListItem>? RowTemplate { get; set; }
	[Parameter] public bool ShowTotal { get; set; } = true;
	[Parameter] public string? TotalDistinctBy { get; set; } // Чтобы в итого не попадали одинаковые заказы 
	// (например, если в таблице один и тот же заказ с разными статусами - история операций)


	private List<ColumnInfo> columnInfos = [];
	private List<ColumnSetting> columnSettings = [];
	private bool isColumnsOpen;

	protected override async Task OnInitializedAsync()
	{
		columnInfos = ColumnHelper.GetColumns<TListItem>();

		columnSettings = ColumnSettings?.Any() == true
			? ColumnSettings
			: TableSettingsManager.GetDefaultSettings<TListItem>();

		await LoadSettings();
	}

	private async Task LoadSettings()
	{
		try
		{
			var tableSettings = await JSRuntime.InvokeAsync<string>("localStorage.getItem", $"table_settings_{TableKey}");
			columnSettings = TableSettingsManager.GetSettings<TListItem>(tableSettings);
		}
		catch
		{
			columnSettings = TableSettingsManager.GetDefaultSettings<TListItem>();
		}
	}

	private List<(ColumnInfo Info, ColumnSetting Setting)> BuildVisibleColumns() =>
		columnSettings
		.Where(s => s.Visible)
		.Join(columnInfos, s => s.PropertyName, i => i.PropertyName, (s, i) => (Info: i, Setting: s))
		.OrderBy(x => x.Setting.Order)
		.ToList();

	private async Task ToggleColumn(string propertyName, bool visible)
	{
		var setting = columnSettings.FirstOrDefault(s => s.PropertyName == propertyName);
		if (setting != null) setting.Visible = visible;
		await SaveSettings();
		StateHasChanged();
	}

	private async Task MoveColumn(string propertyName, int direction)
	{
		var setting = columnSettings.FirstOrDefault(s => s.PropertyName == propertyName);
		if (setting == null) return;
		var ordered = columnSettings.OrderBy(s => s.Order).ToList();
		var idx = ordered.IndexOf(setting);
		var newIdx = idx + direction;
		if (newIdx < 0 || newIdx >= ordered.Count) return;
		(ordered[idx].Order, ordered[newIdx].Order) = (ordered[newIdx].Order, ordered[idx].Order);
		await SaveSettings();
		StateHasChanged();
	}

	private async Task ResetColumns()
	{
		columnSettings = ColumnHelper.GetDefaultSettings<TListItem>();
		await SaveSettings();
		StateHasChanged();
	}

	private async Task SaveSettings()
	{
		var json = TableSettingsManager.Serialize(columnSettings);
		await JSRuntime.InvokeVoidAsync("localStorage.setItem", $"table_settings_{TableKey}", json);
	}

	private IEnumerable<TListItem> ItemsForTotal =>
	string.IsNullOrEmpty(TotalDistinctBy)
		? Items
		: Items
			.GroupBy(i => typeof(TListItem).GetProperty(TotalDistinctBy!)?.GetValue(i))
			.Select(g => g.First());


	private decimal GetColumnTotal(ColumnInfo column)
	{
		if (!column.ShowTotal) return 0;

		return ItemsForTotal.Sum(item =>
		{
			var prop = typeof(TListItem).GetProperty(column.PropertyName);
			var value = prop?.GetValue(item);
			return value switch
			{
				int i => i,
				decimal d => d,
				double d => (decimal)d,
				_ => 0
			};
		});
	}
}