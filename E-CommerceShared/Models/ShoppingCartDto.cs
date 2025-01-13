using System.Text.Json.Serialization;

namespace E_CommerceShared.Models
{
	public class ShoppingCartDto
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public ICollection<CartItemDto> CartItems { get; set; }
	}
}
