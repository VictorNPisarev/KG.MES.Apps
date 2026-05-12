using System.Text.Json.Serialization;

namespace KG.MES.Shared.Models.Dto
{
	public class SupplyCondition
	{
		[JsonPropertyName("id")]
		public string Id { get; set; } = string.Empty;

		[JsonPropertyName("condition_code")]
		public string ConditionCode { get; set; } = string.Empty;
	}

	public static class SupplyConditionExtensions
	{
		public static string DisplayName(this SupplyCondition condition) => condition.ConditionCode?.ToLower() switch
		{
			"not_available" => "Нет в наличии",
			"delayed" => "Задерживается",
			"ordered" => "Заказан",
			"in_stock" => "На складе",
			_ => condition.ConditionCode ?? "—"
		};
	}
}