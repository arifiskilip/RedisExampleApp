using Microsoft.EntityFrameworkCore;
using RedisExampleApp.API.Entities;

namespace RedisExampleApp.API.Contexts
{
	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		// Seed Data
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Product>().HasData(
				new Product() { Id = 1, Name = "Product 1", Price = 100, UnitsInStock = 50 },
				new Product() { Id = 2, Name = "Product 2", Price = 52, UnitsInStock = 65 },
				new Product() { Id = 3, Name = "Product 3", Price = 65, UnitsInStock = 25 },
				new Product() { Id = 4, Name = "Product 4", Price = 45, UnitsInStock = 45 },
				new Product() { Id = 5, Name = "Product 5", Price = 89, UnitsInStock = 78 }
				);
		}
		public DbSet<Product> Products { get; set; }
    }
}
