using System;
using System.Threading.Tasks;

namespace SharedKernel.Redis;
public interface IRedisService
{
    Task<T> GetAsync<T>(string key);
    Task<string> GetAsync(string key);
    Task SetAsync(string key, string value, TimeSpan expiration = default);
    Task SetAsync<T>(string key, T obj, TimeSpan expiration = default);
    Task Remove(string key);
}
