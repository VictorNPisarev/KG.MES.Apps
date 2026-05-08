
using KG.MES.Shared.Models.Dto;

namespace KG.MES.Shared.Services
{
	public class OrderSupplyDtoFactory
	{
		private readonly SupplyService _supplyService;

		public OrderSupplyDtoFactory(SupplyService supplyService)
		{
			_supplyService = supplyService;
		}

		public async Task<List<OrderSupplyDto>> CreateAsync(Guid orderId, ProductionApiService api)
		{
			var types = await _supplyService.GetTypesAsync();
			var conditions = await _supplyService.GetConditionsAsync();
			var supplies = await api.GetOrderSuppliesAsync(orderId);

			foreach (var s in supplies)
				s.Enrich(types, conditions);

			return supplies;
		}
	}
}