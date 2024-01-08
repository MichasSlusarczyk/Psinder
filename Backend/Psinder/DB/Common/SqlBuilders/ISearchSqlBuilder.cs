using Psinder.DB.Common.Searching;
using static Dapper.SqlBuilder;

namespace Psinder.DB.Common.SqlBuilders;

public interface ISearchSqlBuilder<TFilters, TSortEnum>
    where TFilters : class
    where TSortEnum : Enum
{
    ISearchSqlBuilder<TFilters, TSortEnum> AddSelect();

    ISearchSqlBuilder<TFilters, TSortEnum> AddJoins();

    ISearchSqlBuilder<TFilters, TSortEnum> AddFrom();

    ISearchSqlBuilder<TFilters, TSortEnum> AddFilters(TFilters filters);

    ISearchSqlBuilder<TFilters, TSortEnum> AddSorting(SortInfo<TSortEnum>? sortInfo);

    ISearchSqlBuilder<TFilters, TSortEnum> AddPaging(PageInfo sortInfo);

    ISearchSqlBuilder<TFilters, TSortEnum> AddTotalSizeQuerySelect();

    Template BuildSelectQuery();

    Template BuildTotalSizeQuery();

    ISearchSqlBuilder<TFilters, TSortEnum> AddContextFilter(long? id);
}
