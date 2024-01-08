using Psinder.DB.Common.Entities;

namespace Psinder.DB.Common.Services;

public interface ICacheService
{
    public Task ClearCacheForKey(string key, CancellationToken cancellationToken);

    public Task ClearCacheForCategory(ConfigurationCategory category, CancellationToken cancellationToken);

    public Task<List<Configuration>> GetConfigurationsByCategory(ConfigurationCategory category, CancellationToken cancellationToken);

    public Task<Configuration> GetConfigurationByItem(ConfigurationCategory category, ConfigurationItem item, CancellationToken cancellationToken);

    public Task<string> GetConfigurationStringValueByItem(ConfigurationCategory category, ConfigurationItem item, CancellationToken cancellationToken);

    public Task<long> GetConfigurationIntValueByItem(ConfigurationCategory category, ConfigurationItem item, CancellationToken cancellationToken);

}
