using System.Text.Json.Serialization;

namespace E_CommerceShared.Models
{
	public class ShoppingCart
	{
		public int Id { get; set; }
		public string UserId { get; set; }
		public ICollection<CartItem> CartItems { get; set; }
	}
}
