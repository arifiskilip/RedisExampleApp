using RedisExampleApp.API.Entities;
using RedisExampleApp.API.Repositorires;

namespace RedisExampleApp.API.Services
{
	public class ProductService : IProductService
	{
		private readonly IProductRepository _productRepository;

		public ProductService(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		public async Task<Product> CreateAsync(Product product)
		{
			await _productRepository.CreateAsync(product: product);
			return product;
		}

		public async Task<List<Product>> GetAllAsync()
		{
			return await _productRepository.GetAllAsync();
		}

		public async Task<Product> GetAsync(int id)
		{
			return await _productRepository.GetAsync(id: id);
		}
	}
}
