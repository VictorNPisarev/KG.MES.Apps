using KG.MES.Shared.Models.Dto;
using Mapster;

namespace KG.MES.Shared.Models.ViewModels;

public class OrderSupplyViewModel
{
	public string SupplyTypeId { get; private set; } = string.Empty;
	public string? SupplyConditionId { get; private set; }
	public string? Comment { get; set; }
	public Guid? CommentId { get; set; }

	// Справочные данные (заполняются извне)
	// Отображаемые данные (заполняются при маппинге извне)
	public string? SupplyTypeName { get; set; }
	public string? SupplyConditionName { get; set; }
	public string? SupplyConditionCode { get; set; }

	public OrderSupplyViewModel () {}

	public OrderSupplyViewModel(OrderSupplyDto orderSupplyDto)
	{
		orderSupplyDto.Adapt(this);
	}

	public OrderSupplyDto ToDto() => this.Adapt<OrderSupplyDto>();

	public void SetCondition(string conditionId, SupplyConditionDto? condition = null)
	{
		SupplyConditionId = conditionId;

		SupplyConditionName = condition?.DisplayName();
		SupplyConditionCode = condition?.ConditionCode;
	}
}