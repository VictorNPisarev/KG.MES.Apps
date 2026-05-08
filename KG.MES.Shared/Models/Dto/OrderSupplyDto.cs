using System.Text.Json.Serialization;
using KG.MES.Shared.Attributes;

namespace KG.MES.Shared.Models.Dto
{
	public class OrderSupplyDto
	{
		[JsonPropertyName("supply_type_id")]
		public string SupplyTypeId { get; set; } = string.Empty;

		[JsonPropertyName("supply_condition_id")]
		public string? SupplyConditionId { get; set; }

		[JsonPropertyName("expected_date")]
		public DateTime? ExpectedDate { get; set; }

		[JsonPropertyName("quantity")]
		public double? Quantity { get; set; }

		[JsonPropertyName("comment")]
		public string? Comment { get; set; }

		// Справочные данные (заполняются извне)
		[JsonIgnore] public SupplyType? SupplyType { get; private set; }
		[JsonIgnore] public SupplyCondition? SupplyCondition { get; private set; }
		//[JsonIgnore] public string? ConditionBadge { get; private set; }

		public void Enrich(List<SupplyType> types, List<SupplyCondition> conditions)
		{
			SupplyType = types.FirstOrDefault(t => t.Id == SupplyTypeId);
			SupplyCondition = conditions.FirstOrDefault(c => c.Id == SupplyConditionId);
		}

	}
}