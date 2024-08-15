using RedisExampleApp.API.Entities;

namespace RedisExampleApp.API.Services
{
	public interface IProductService
	{
		Task<Product> CreateAsync(Product product);
		Task<List<Product>> GetAllAsync();
		Task<Product> GetAsync(int id);
	}
}
