using Psinder.DB.Common.Extensions;
using System.Web;

namespace Psinder.DB.Common.Searching;

public abstract class SearchRequest<TFilters, TSortEnum>
    where TFilters : class
    where TSortEnum : Enum
{
    public SearchRequest(
        TFilters? filters = null, 
        SortInfo<TSortEnum>? sorting = null, 
        PageInfo? paging = null)
    {
        Filters = filters;
        Sorting = sorting;
        Paging = paging;
    }

    public TFilters? Filters { get; set; }

    public SortInfo<TSortEnum>? Sorting { get; set; }

    public PageInfo? Paging { get; }

    public virtual string ToQuery()
    {
        var query = HttpUtility.ParseQueryString(string.Empty);
        query.AddQueryParameter("orderBy", Sorting?.CreateSortExpression());
        query.AddQueryParameter("page", Paging?.Page);
        query.AddQueryParameter("pageSize", Paging?.PageSize);
        var queryString = query.ToString();

        return queryString ?? string.Empty;
    }
}
