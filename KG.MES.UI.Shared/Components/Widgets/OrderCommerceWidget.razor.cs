using KG.MES.Shared.Interfaces;
using KG.MES.Shared.Models.Dto;
using KG.MES.Shared.Services;
using KG.MES.UI.Shared.Interfaces;
using Microsoft.AspNetCore.Components;

namespace KG.MES.UI.Shared.Components.Widgets;

public partial class OrderCommerceWidget<TOrder> : ComponentBase, ISavableWidget
{
	[Parameter] public Guid OrderId { get; set; }

	[Inject] private ProductionApiService ApiService { get; set; } = null!;
	[Inject] private ISocketService SocketService { get; set; } = null!;
	[Inject] private IEventAggregator EventAggregator { get; set; } = null!;

	private OrderCommerceDto? commerce;
	public bool isLoading = true;

	protected override async Task OnInitializedAsync()
	{
		commerce = await ApiService.GetCommerceAsync(OrderId);
		isLoading = false;
	}

	public bool HasUnsavedChanges() => false;
	public Task SaveAllAsync() => Task.CompletedTask;
}