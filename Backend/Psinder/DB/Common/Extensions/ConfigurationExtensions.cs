using Psinder.DB.Common.Entities;

namespace Psinder.DB.Common.Extensions;

public static class ConfigurationExtensions
{
    public static Configuration GetSingleConfiguration(this List<Configuration> configurations, ConfigurationCategory category, ConfigurationItem item)
    {
        var configurationItem = configurations.FirstOrDefault(x => x.Id == (long)item);

        if (configurationItem == null)
        {
            throw new InvalidOperationException("The specified configuration could not be found.");
        }

        if (configurationItem.ConfigurationCategoryId != (long)category)
        {
            throw new InvalidOperationException("Incompatible configuration type");

        }

        return configurationItem;
    }

    public static string GetConfigurationStringValue(this List<Configuration> configurations, ConfigurationCategory category, ConfigurationItem item)
    {
        var configurationItem = configurations.GetSingleConfiguration(category, item);

        if (configurationItem.Value == null)
        {
            throw new InvalidOperationException("The specified configuration could not be found.");
        }

        return configurationItem.Value;
    }

    public static long GetConfigurationLongValue(this List<Configuration> configurations, ConfigurationCategory category, ConfigurationItem item)
    {
        var configurationItem = configurations.GetConfigurationStringValue(category, item);

        try
        {
            return long.Parse(configurationItem);
        }
        catch (Exception)
        {
            throw new InvalidOperationException("The configuration item should by numeric type.");
        }
    }
}
