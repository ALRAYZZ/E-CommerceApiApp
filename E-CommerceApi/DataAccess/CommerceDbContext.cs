using E_CommerceShared.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceApi.DataAccess
{
	public class CommerceDbContext : IdentityDbContext
	{
		public CommerceDbContext(DbContextOptions<CommerceDbContext> options) : base(options)
		{
		}
		public DbSet<Product> Products { get; set; }
		public DbSet<OrderItem> Items { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<CartItem> OrderItems { get; set; }
		public DbSet<ShoppingCart> ShoppingCarts { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			// ShoppingCart and User (ASP.NET Identity)
			modelBuilder.Entity<ShoppingCart>()
				.HasOne<IdentityUser>() // Assuming you're using ASP.NET Identity's User
				.WithMany()
				.HasForeignKey(c => c.UserId);

			// CartItem and ShoppingCart
			modelBuilder.Entity<CartItem>()
				.HasOne(ci => ci.ShoppingCart)
				.WithMany(sc => sc.CartItems)
				.HasForeignKey(ci => ci.ShoppingCartId);

			// CartItem and Product
			modelBuilder.Entity<CartItem>()
				.HasOne(ci => ci.Product)
				.WithMany()
				.HasForeignKey(ci => ci.ProductId);

			// Order and User
			modelBuilder.Entity<Order>()
				.HasOne<IdentityUser>() // Assuming you're using ASP.NET Identity's User
				.WithMany()
				.HasForeignKey(o => o.UserId);

			// OrderItem and Order
			modelBuilder.Entity<OrderItem>()
				.HasOne(oi => oi.Order)
				.WithMany(o => o.OrderItems)
				.HasForeignKey(oi => oi.OrderId);

			// OrderItem and Product
			modelBuilder.Entity<OrderItem>()
				.HasOne(oi => oi.Product)
				.WithMany()
				.HasForeignKey(oi => oi.ProductId);
		}
	}
}
