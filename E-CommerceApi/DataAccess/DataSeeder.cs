//using E_CommerceShared.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;

//namespace E_CommerceApi.DataAccess
//{
//	public class DataSeeder
//	{
//		public static void Seed(IServiceProvider serviceProvider)
//		{
//			using (var context = new CommerceDbContext(
//				serviceProvider.GetRequiredService<DbContextOptions<CommerceDbContext>>()))
//			{
//				var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
//				var logger = serviceProvider.GetRequiredService<ILogger<DataSeeder>>();
//				context.Database.EnsureCreated();

//				if (context.Products.Any())
//				{
//					return;
//				}

//				var user = userManager.Users.FirstOrDefault(u => u.UserName == "admin");
//				if (user == null)
//				{
//					logger.LogError("User not found");
//					throw new Exception("User not found");
//				}



//				var product1 = new ProductDto()
//				{
//					Name = "Product 1",
//					Description = "Description 1",
//					Price = 10,
//					StockQuantity = 100
//				};
//				var product2 = new ProductDto()
//				{
//					Name = "Product 2",
//					Description = "Description 2",
//					Price = 20,
//					StockQuantity = 200
//				};

//				context.Products.AddRange(product1, product2);
//				context.SaveChanges();


//				var shoppingCart = new ShoppingCart()
//				{
//					UserId = user.Id,
//					CartItems = new List<CartItem>()
//					{
//						new CartItem()
//						{
//							ProductId = product1.Id,
//							Quantity = 2
//						},
//						new CartItem()
//						{
//							ProductId = product2.Id,
//							Quantity = 1
//						}
//					}
//				};

//				context.ShoppingCarts.Add(shoppingCart);

//				context.SaveChanges();
//			}
//		}
//	}
//}
