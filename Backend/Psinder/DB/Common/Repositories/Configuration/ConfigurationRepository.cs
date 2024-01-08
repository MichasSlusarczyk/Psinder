using Microsoft.EntityFrameworkCore;
using Psinder.DB.Common.Entities;
using Psinder.DB.Common.Repositories.UnitOfWorks;

namespace Psinder.DB.Common.Repositories.Configurations;

public class ConfigurationRepository : IConfigurationRepository
{
    private readonly IUnitOfWork _unitOfWork;

    public ConfigurationRepository(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<Configuration>> GetConfigurationsByCategory(ConfigurationCategory category, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.ConfigurationsEntity
            .Where(x => x.ConfigurationCategoryId == (long)category)
            .ToListAsync(cancellationToken);
    }

    public async Task<Configuration> GetConfigurationByItem(ConfigurationItem item, CancellationToken cancellationToken)
    {
        return await _unitOfWork.DatabaseContext.ConfigurationsEntity
            .Where(x => x.ConfigurationItemId == (long)item)
            .SingleAsync(cancellationToken);
    }
}