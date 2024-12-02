
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Aspire.Hosting.Redis;
using Microsoft.AspNetCore.Builder;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStackExchangeRedisCache(options =>
{
	options.Configuration = "localhost:6379";
});

builder.Services.AddHybridCache(options =>
{
	options.MaximumPayloadBytes = 1024 * 1024; // 1 MB max payload size
	options.MaximumKeyLength = 1024; // Max key length
	options.DefaultEntryOptions = new HybridCacheEntryOptions
	{
		Expiration = TimeSpan.FromMinutes(3), // Cache expiration time
		LocalCacheExpiration = TimeSpan.FromMinutes(3) // Local cache expiration
	};
	
});

// Register CustomHybridCacheService
builder.Services.AddSingleton<ICustomHybridCacheService, CustomHybridCacheService>();

var app = builder.Build();

// Minimal API endpoint
app.MapGet("/cached-data/{key}",
async (string key, ICustomHybridCacheService cacheService, CancellationToken token) => 
{ 
	var result = await cacheService.GetCachedDataAsync(key, token); 
	return Results.Ok(result); 

});

app.Run();
