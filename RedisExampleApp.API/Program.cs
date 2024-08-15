using Microsoft.EntityFrameworkCore;
using RedisExampleApp.API.Contexts;
using RedisExampleApp.API.Repositorires;
using RedisExampleApp.API.Services;
using RedisExampleApp.Cache;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add Db
builder.Services.AddDbContext<AppDbContext>(opt =>
{
	opt.UseInMemoryDatabase("MyDb");
});

// IoC
builder.Services.AddScoped<IProductRepository>(sp =>
{
	var appDbContext = sp.GetRequiredService<AppDbContext>();
	var productRepository = new ProductRepository(appDbContext);
	var redisService = sp.GetRequiredService <RedisService>();
	return new ProductRepositoryWithCacheDecorator(productRepository, redisService);
});
builder.Services.AddScoped<RedisService>(sp =>
{
	return new RedisService(builder.Configuration["CacheOptions:Url"]);
});
builder.Services.AddScoped<IDatabase>(sp =>
{
	var redisService = sp.GetRequiredService<RedisService>();
	return redisService.GetDb(dbIndex: 0);
});
builder.Services.AddScoped<IProductService, ProductService>();


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();
	dbContext.Database.EnsureCreated();
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
