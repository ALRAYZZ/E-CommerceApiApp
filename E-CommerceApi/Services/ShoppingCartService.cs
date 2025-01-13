using E_CommerceApi.DataAccess;
using E_CommerceShared.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;	

namespace E_CommerceApi.Services
{
	public class ShoppingCartService : IShoppingCartService
	{
		private readonly CommerceDbContext	_context;
		private readonly ILogger _logger;

		public ShoppingCartService(CommerceDbContext context, ILogger<ShoppingCartService> logger)
		{
			_context = context;
			_logger = logger;
		}

		public async Task AddToCartAsync(string userId, int productId, int quantity)
		{
			// Find the shopping cart for the user
			var cart = await _context.ShoppingCarts
				.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.UserId == userId);
			// If the cart doesn't exist, create a new one
			if (cart == null)
			{
				cart = new ShoppingCart
				{
					UserId = userId,
					CartItems = new List<CartItem>()
				};
				_context.ShoppingCarts.Add(cart);
			}

			// Find the item in the cart
			var orderItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
			// If the item doesn't exist, create a new one
			if (orderItem == null)
			{
				orderItem = new CartItem
				{
					ProductId = productId,
					Quantity = quantity
				};
				cart.CartItems.Add(orderItem);
			}
			// Otherwise, update the quantity
			else
			{
				orderItem.Quantity += quantity;
			}
			await _context.SaveChangesAsync();
		}

		public async Task<ShoppingCartDto> GetCartByUserIdAsync(string userId)
		{
			_logger.LogInformation($"Getting cart for user {userId}");
			var cart = await _context.ShoppingCarts
				.Include(c => c.CartItems)
				.ThenInclude(i => i.Product)
				.FirstOrDefaultAsync(c => c.UserId == userId);
			_logger.LogInformation($"Shopping cart found: {cart != null}");
			if (cart == null)
			{
				return null;
			}
			return new ShoppingCartDto
			{
				Id = cart.Id,
				UserId = cart.UserId,
				CartItems = cart.CartItems.Select(i => new CartItemDto
				{
					Id = i.Id,
					ProductId = i.ProductId,
					Product = new ProductDto
					{
						Id = i.Product.Id,
						Name = i.Product.Name,
						Description = i.Product.Description,
						Price = i.Product.Price
					},
					Quantity = i.Quantity
				}).ToList()
			};
		}

		public async Task RemoveFromCartAsync(string userId, int productId)
		{
			var cart = await _context.ShoppingCarts
				.Include(c => c.CartItems)
				.FirstOrDefaultAsync(c => c.UserId == userId);

			if (cart == null)
			{
				return;
			}

			var orderItem = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
			if (orderItem != null)
			{
				cart.CartItems.Remove(orderItem);
				await _context.SaveChangesAsync();
			}
		}
	}
}
