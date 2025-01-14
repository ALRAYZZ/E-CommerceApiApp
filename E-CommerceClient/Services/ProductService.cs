using E_CommerceShared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_CommerceClient.Services
{
	public interface IProductService
	{
		Task<List<ProductDto>> GetProductsAsync();
	}

	public class ProductService : IProductService
	{
		public Task<List<ProductDto>> GetProductsAsync()
		{
			var products = new List<ProductDto>()
			{
				new ProductDto { Id = 10, Name = "Product 10", Description = "Description 1", Price = 10, StockQuantity = 100 },
				new ProductDto { Id = 20, Name = "Product 20", Description = "Description 2", Price = 20, StockQuantity = 200 },
				new ProductDto { Id = 30, Name = "Product 30", Description = "Description 3", Price = 30, StockQuantity = 300 },

			};

			return Task.FromResult(products);
		}
	}
}
