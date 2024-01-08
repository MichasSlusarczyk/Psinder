namespace Psinder.DB.Common.Services;

public static class CacheKeys
{
    public static string ConfigurationCategory(long categoryId) => $"Configuration_category_{categoryId}";
    
    public static string Dictionary<T>(T dictionary) => $"{dictionary?.GetType().Name}_{Convert.ToInt64(dictionary)}";
}
