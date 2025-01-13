using E_CommerceApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_CommerceApi.Controllers
{
	[Authorize]
	[ApiController]
	[Route("api/[controller]")]
	public class ShoppingCartController : ControllerBase
	{
		private readonly IShoppingCartService _shoppingCartService;

		public ShoppingCartController(IShoppingCartService shoppingCartService)
		{
			_shoppingCartService = shoppingCartService;
		}


		[HttpPost("add")]
		public async Task<IActionResult> AddtoCart(int productId, int quantity)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
			{
				return Unauthorized();
			}

			await _shoppingCartService.AddToCartAsync(userId, productId, quantity);
			return Ok();
		}
		[HttpGet("{userId}")]
		public async Task<IActionResult> GetCart(string userId)
		{
			var cart = await _shoppingCartService.GetCartByUserIdAsync(userId);
			if (cart == null)
			{
				return NotFound("No Cart found");
			}
			
			return Ok(cart);
		}
		[HttpDelete("remove")]
		public async Task<IActionResult> RemoveFromCart(int productId)
		{
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			if (userId == null)
			{
				return Unauthorized();
			}
			if (!int.TryParse(userId, out int parsedUserId))
			{
				return BadRequest("Invalid user ID");
			}

			await _shoppingCartService.RemoveFromCartAsync(userId, productId);
			return Ok();
		}
	}
}
