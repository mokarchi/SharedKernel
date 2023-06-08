using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using SharedKernel.Utils;

namespace SharedKernel.Redis;
public class RedisService : IRedisService
{
    private readonly IDistributedCache _distributedCache;

    public RedisService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var redis = await _distributedCache.GetStringAsync(key);
        return redis == null ? default : JsonUtils.Deserialize<T>(redis);
    }

    public async Task<string> GetAsync(string key)
    {
        return await _distributedCache.GetStringAsync(key);
    }

    public async Task Remove(string key)
    {
        await _distributedCache.RemoveAsync(key);
    }

    public async Task SetAsync(string key, string value, TimeSpan expiration = default)
    {
        if (expiration != default)
        {
            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = expiration
            };
            await _distributedCache.SetStringAsync(key, value, options);
            return;
        }

        await _distributedCache.SetStringAsync(key, value);
    }

    public async Task SetAsync<T>(string key, T obj, TimeSpan expiration = default)
    {
        var value = JsonUtils.Serialize(obj);
        if (expiration != default)
        {
            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = expiration
            };
            await _distributedCache.SetStringAsync(key, value, options);
            return;
        }

        await _distributedCache.SetStringAsync(key, value);
    }
}
