using Psinder.DB.Common.Extensions;
using Psinder.DB.Common.Searching;
using System.Linq.Expressions;

namespace Psinder.API.Common.Helpers;

public static class ListHelper<T, TSortEnum>
    where T : class
    where TSortEnum : Enum
{
    public static List<T> OrderBy(List<T> list, SortInfo<TSortEnum> sortInfo)
    {
        var orderStatement = GetOrderStatement(sortInfo.ByColumn!);

        return sortInfo.Direction switch
        {
            SortDirections.ASC => list.OrderBy(orderStatement).ToList(),
            SortDirections.DESC => list.OrderByDescending(orderStatement).ToList(),
            _ => throw new NotSupportedException($"Incorrect Sort direction: {sortInfo.Direction}.")
        };
    }

    public static List<T> Page(List<T> list, PageInfo pageInfo)
    {
        return list.Skip(pageInfo!.Offset).Take(pageInfo!.PageSize).ToList();
    }

    private static Func<T, object> GetOrderStatement(Enum e)
    {
        var attrName = e.ValueToString();
        var type = Expression.Parameter(typeof(T), attrName);
        var property = Expression.PropertyOrField(type, attrName);
        return Expression.Lambda<Func<T, object>>(Expression.Convert(property, typeof(object)), type).Compile();
    }
}
