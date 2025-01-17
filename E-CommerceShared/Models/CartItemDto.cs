﻿namespace E_CommerceShared.Models
{
	public class CartItemDto
	{
		public int Id { get; set; }
		public int ProductId { get; set; }
		public ProductDto Product { get; set; }
		public int Quantity { get; set; }
		public int ShoppingCartId { get; set; }
		public ShoppingCart ShoppingCart { get; set; }
	}
}
