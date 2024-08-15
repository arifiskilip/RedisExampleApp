using RedisExampleApp.API.Entities;

namespace RedisExampleApp.API.Repositorires
{
	public interface IProductRepository
	{
		Task<Product> CreateAsync(Product product);
		Task<List<Product>> GetAllAsync();
		Task<Product> GetAsync(int id);
	}
}
