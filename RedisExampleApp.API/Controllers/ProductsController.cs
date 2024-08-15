using Microsoft.AspNetCore.Mvc;
using RedisExampleApp.API.Entities;
using RedisExampleApp.API.Repositorires;

namespace RedisExampleApp.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{
		private readonly IProductRepository _productRepository;

		public ProductsController(IProductRepository productRepository)
		{
			_productRepository = productRepository;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var result = await _productRepository.GetAllAsync();
			return Ok(result);
		}
		[HttpGet]
		public async Task<IActionResult> GetById([FromQuery] int id)
		{
			var result = await _productRepository.GetAsync(id:id);
			return Ok(result);
		}
		[HttpPost]
		public async Task<IActionResult> GetById([FromBody] Product product)
		{
			var result = await _productRepository.CreateAsync(product: product);
			return Ok(result);
		}
	}
}
