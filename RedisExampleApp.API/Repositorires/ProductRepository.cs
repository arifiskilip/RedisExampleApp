using Microsoft.EntityFrameworkCore;
using RedisExampleApp.API.Contexts;
using RedisExampleApp.API.Entities;

namespace RedisExampleApp.API.Repositorires
{
	public class ProductRepository : IProductRepository
	{
		private readonly AppDbContext _context;
		private DbSet<Product> _products;

		public ProductRepository(AppDbContext context)
		{
			_context = context;
			_products = _context.Set<Product>();
		}

		public async Task<Product> CreateAsync(Product product)
		{
			await _products.AddAsync(product);
			await _context.SaveChangesAsync();
			return product;
		}

		public async Task<List<Product>> GetAllAsync()
		{
			return await _products.ToListAsync();
		}

		public async Task<Product?> GetAsync(int id)
		{
			return await _products.FirstOrDefaultAsync(predicate: x => x.Id == id);
		}
	}
}
