using Microsoft.EntityFrameworkCore.Storage;
using RedisExampleApp.API.Entities;
using RedisExampleApp.Cache;
using StackExchange.Redis;
using System.Text.Json;
using IDatabase = StackExchange.Redis.IDatabase;

namespace RedisExampleApp.API.Repositorires
{
	// Decorator Design Pattern
	public class ProductRepositoryWithCacheDecorator : IProductRepository
	{
		private const string productKey = "productCaches"; 
		private readonly IProductRepository _repository;
		private readonly RedisService _redisService;
		private readonly IDatabase _cacheRepository;

		public ProductRepositoryWithCacheDecorator(IProductRepository repository, RedisService redisService)
		{
			_repository = repository;
			_redisService = redisService;
			_cacheRepository = _redisService.GetDb(dbIndex: 0);
		}

		public async Task<Product> CreateAsync(Product product)
		{
			var newProduct = await _repository.CreateAsync(product: product);
			if (await _cacheRepository.KeyExistsAsync(key:productKey))
			{
				await _cacheRepository.HashSetAsync(key: productKey, hashField: product.Id, value: JsonSerializer.Serialize(product));
			}
			return newProduct;
		}

		public async Task<List<Product>> GetAllAsync()
		{
			if (!await _cacheRepository.KeyExistsAsync(productKey))
			{
				return await LoadToCacheFromDbAsync();
			}
			var products = new List<Product>();

			var cachePrdocuts = await _cacheRepository.HashGetAllAsync(key:productKey);
            foreach (var item in cachePrdocuts.ToList())
            {
				var product = JsonSerializer.Deserialize<Product>(item.Value);
				products.Add(product);
            }
			return products;
        }

		public async Task<Product> GetAsync(int id)
		{
			if (await _cacheRepository.KeyExistsAsync(productKey))
			{
				var product = await _cacheRepository.HashGetAsync(key: productKey, hashField: id);
				return product.HasValue ? JsonSerializer.Deserialize<Product>(product) : null;
			}
			var products = await LoadToCacheFromDbAsync();
			return products.FirstOrDefault(predicate:x=>x.Id == id);
		}


		// Load Cache
		private async Task<List<Product>> LoadToCacheFromDbAsync()
		{
			var products = await _repository.GetAllAsync();
			products.ForEach(p =>
			{
				_cacheRepository.HashSetAsync(key: productKey, hashField: p.Id, value: JsonSerializer.Serialize(p));
			});
			return products;
		}
	}
}
