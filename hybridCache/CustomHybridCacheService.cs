using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using System.Threading.Tasks;

public interface ICustomHybridCacheService
{
	Task<CachedDataResult> GetCachedDataAsync(string key, CancellationToken token = default);
}

public class CustomHybridCacheService : ICustomHybridCacheService
{
	private readonly HybridCache _cache;
	private readonly IMemoryCache _memoryCache;
	private readonly IDistributedCache _distributedCache;

	public CustomHybridCacheService(HybridCache cache, IMemoryCache memoryCache, IDistributedCache distributedCache)
	{
		_cache = cache;
		_memoryCache = memoryCache;
		_distributedCache = distributedCache;
	}

	public async Task<CachedDataResult> GetCachedDataAsync(string key, CancellationToken token = default)
	{
		bool isCached = false;
		string cacheSource = "None";

		var data = await _cache.GetOrCreateAsync<string, string>(
			key,
			key, // Passing the state which is the key in this case
			async (key, token) =>
			{
				isCached = false;
				return await GetDataAsync(key, token);
			},
			cancellationToken: token
		);
		// Check if data exists in Memory Cache
		if (_memoryCache.TryGetValue(key, out _))
		{
			isCached = true;
			cacheSource = "Memory";
		}

		// Check if data exists in Distributed Cache
		 if (await _distributedCache.GetStringAsync(key) != null)
		{
			isCached = true;
			cacheSource += " + Redis";

		}

		return new CachedDataResult(data, isCached, cacheSource);
	}

	private async Task<string> GetDataAsync(string key, CancellationToken token)
	{
		await Task.Delay(500, token);  // Simulate a slow data source like a database
		return $"Data for cache entry with key: {key}";
	}
}

public class CachedDataResult
{
	public string Data { get; }
	public bool IsCached { get; }
	public string CacheSource { get; }

	public CachedDataResult(string data, bool isCached, string cacheSource)
	{
		Data = data;
		IsCached = isCached;
		CacheSource = cacheSource;
	}
}
