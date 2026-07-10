using System.Text.Json.Serialization;
using KG.MES.Shared.Attributes;

namespace KG.MES.Shared.Models.Dto
{
	public class OrderCommerceDto
	{
		[JsonPropertyName("id")]
		public Guid Id { get; set; }

		[JsonPropertyName("order_id")]
		public Guid OrderId { get; set; }

		[JsonPropertyName("manager")]
		[Column("Менеджер", Order = 0)]
		public string? Manager { get; set; }

		[JsonPropertyName("customer_name")]
		[Column("Контрагент", Order = 1)]
		public string? CustomerName { get; set; }

		[JsonPropertyName("price")]
		[Column("Цена", Order = 2, DisplayFormat = "C")]
		public decimal Price { get; set; }

		[JsonPropertyName("created_at")]
		public DateTime CreatedAt { get; set; }

		[JsonPropertyName("updated_at")]
		public DateTime? UpdatedAt { get; set; }
	}
}