using System.Collections.Specialized;

namespace Psinder.DB.Common.Extensions;

public static class QueryStringExtensions
{
    public static void AddQueryParameter(this NameValueCollection collection, string name, object? value)
    {
        if (value == null)
        {
            return;
        }

        collection[name] = value switch
        {
            Enum => ((int)value).ToString(),
            DateTime => ((DateTime)value).ToString("yyyy-MM-dd"),
            _ => value.ToString()
        };
    }
}
