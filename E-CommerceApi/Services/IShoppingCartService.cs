using E_CommerceShared.Models;

namespace E_CommerceApi.Services
{
	public interface IShoppingCartService
	{
		Task AddToCartAsync(string userId, int productId, int quantity);
		Task<ShoppingCartDto> GetCartByUserIdAsync(string userId);
		Task RemoveFromCartAsync(string userId, int productId);
	}
}
