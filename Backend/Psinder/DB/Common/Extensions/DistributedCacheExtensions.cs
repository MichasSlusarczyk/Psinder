using Microsoft.Extensions.Caching.Distributed;

namespace Psinder.DB.Common.Extensions;

public static class DistributedCacheExtensions
{
    public async static Task SetAsync<T>(
        this IDistributedCache distributedCache,
        string key,
        T value,
        string password,
        string salt,
        string IV,
        DistributedCacheEntryOptions options,
        CancellationToken token = default)
    {
        await distributedCache.SetAsync(key, value.ToByteArray(password, salt, IV), options, token);
    }

    public async static Task<T> GetAsync<T>(
    this IDistributedCache distributedCache,
    string key,
    string password,
    string salt,
    string IV,
    CancellationToken token = default)
    {
        var result = await distributedCache.GetAsync(key, token);
        return result.FromByteArray<T>(password, salt, IV);
    }
}
