using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Psinder.DB.Common.Entities;
using Psinder.DB.Common.Repositories.Configurations;
using Psinder.DB.Common.Extensions;

namespace Psinder.DB.Common.Services;

public class CacheService : ICacheService
{
    private readonly IConfiguration _configuration;
    private readonly IConfigurationRepository _configurationRepository;
    private readonly IDistributedCache _distributedCache;
    private readonly string _key;
    private readonly string _salt;
    private readonly string _IV;

    public CacheService(
        IConfiguration configuration,
        IConfigurationRepository configurationRepository,
        IDistributedCache distributedCache
        )
    {
        _configuration = configuration;
        _configurationRepository = configurationRepository;
        _distributedCache = distributedCache;

        _key = _configuration.GetSection("Redis:AES:Key").Value;
        _salt = _configuration.GetSection("Redis:AES:Salt").Value;
        _IV = _configuration.GetSection("Redis:AES:IV").Value;
    }

    public async Task ClearCacheForKey(string key, CancellationToken cancellationToken)
    {
        await _distributedCache.RemoveAsync(key, cancellationToken);
    }

    public async Task ClearCacheForCategory(ConfigurationCategory category, CancellationToken cancellationToken)
    {
        await _distributedCache.RemoveAsync(CacheKeys.ConfigurationCategory((long)category), cancellationToken);
    }

    public async Task<List<Configuration>> GetConfigurationsByCategory(ConfigurationCategory category, CancellationToken cancellationToken)
    {
        var password = _key;
        var salt = _salt;
        var vector = _IV;

        var fromCache = await _distributedCache.GetAsync<List<Configuration>>(CacheKeys.ConfigurationCategory((long)category), password, salt, vector, cancellationToken);
        
        if (fromCache?.Any() == true)
        {
            return fromCache;
        }
        else
        {
            var fromDb = await _configurationRepository.GetConfigurationsByCategory(category, cancellationToken);
            await _distributedCache.SetAsync(CacheKeys.ConfigurationCategory((long)category), fromDb, password, salt, vector, new DistributedCacheEntryOptions(), cancellationToken);
            
            return fromDb;
        }
    }

    public async Task<Configuration> GetConfigurationByItem(ConfigurationCategory category, ConfigurationItem item, CancellationToken cancellationToken)
    {
        var configurations = await GetConfigurationsByCategory(category, cancellationToken);
        var configuration = configurations.First(x => x.Id == (long)item && x.Enabled == true);

        return configuration;
    }

    public async Task<string> GetConfigurationStringValueByItem(ConfigurationCategory category, ConfigurationItem item, CancellationToken cancellationToken)
    {
        var configuration = await GetConfigurationByItem(category, item, cancellationToken);

        return configuration.Value;
    }

    public async Task<long> GetConfigurationIntValueByItem(ConfigurationCategory category, ConfigurationItem item, CancellationToken cancellationToken)
    {
        var configuration = await GetConfigurationByItem(category, item, cancellationToken);

        return Convert.ToInt64(configuration.Value);
    }
}