using Psinder.DB.Common.Entities;

namespace Psinder.DB.Common.Repositories.Configurations;

public interface IConfigurationRepository
{
    public Task<List<Configuration>> GetConfigurationsByCategory(ConfigurationCategory category, CancellationToken cancellationToken);

    public Task<Configuration> GetConfigurationByItem(ConfigurationItem item, CancellationToken cancellationToken);
}