using Dapper;
using Psinder.DB.Common.Searching;

namespace Psinder.DB.Common.SqlBuilders;

public abstract class BaseSearchSqsBuilder<TFilters, TSortEnum> : BaseSqlBuilder, ISearchSqlBuilder<TFilters, TSortEnum>
    where TFilters : class
    where TSortEnum : Enum
{
    protected readonly string _fromTable;

    protected readonly string? _mainColumnName;

    public BaseSearchSqsBuilder(string fromTable, string ?mainColumnName = null)
    {
        _fromTable = fromTable;
        _mainColumnName = mainColumnName;
    }

    public abstract ISearchSqlBuilder<TFilters, TSortEnum> AddSelect();

    public virtual ISearchSqlBuilder<TFilters, TSortEnum> AddFrom()
    {
        if (string.IsNullOrEmpty(_fromTable))
        {
            throw new Exception("FROM part can't be added with null or empty table name.");
        }

        CreateFromPart(_fromTable);
        return this;
    }

    public virtual ISearchSqlBuilder<TFilters, TSortEnum> AddJoins()
    {
        return this;
    }

    public abstract ISearchSqlBuilder<TFilters, TSortEnum> AddFilters(TFilters filters);

    public virtual ISearchSqlBuilder<TFilters, TSortEnum> AddPaging(PageInfo pageInfo)
    {
        CreatePagingPart(pageInfo);
        AddPagingParameters(pageInfo);

        return this;
    }

    public ISearchSqlBuilder<TFilters, TSortEnum> AddSorting(SortInfo<TSortEnum>? sortInfo)
    {
        AddSorting<TSortEnum>(sortInfo);
        return this;
    }

    public ISearchSqlBuilder<TFilters, TSortEnum> AddTotalSizeQuerySelect()
    {
        if (string.IsNullOrEmpty(_mainColumnName))
        {
            throw new Exception("Total size query SELECT part can't be added with null or empty main column name.");
        }

        CreateTotalSizeSelectPart(_mainColumnName);

        return this;
    }

    public SqlBuilder.Template BuildSelectQuery()
    {
        return _sqlBuilder.AddTemplate($@"
            SELECT /**select**/
            {_fromPart}
            /**join**/
            /**innerjoin**/
            /**leftjoin**/
            /**rightjoin**/
            /**where**/
            /**orderby**/
            {_pagingPart}");
    }

    public SqlBuilder.Template BuildTotalSizeQuery()
    {
        return _sqlBuilder.AddTemplate($@"
            {_totalSizeSelectPartPart}
            {_fromPart}
            /**join**/
            /**innerjoin**/
            /**leftjoin**/
            /**rightjoin**/
            /**where**/");
    }

    public ISearchSqlBuilder<TFilters, TSortEnum> AddContextFilter(long? id)
    {
        throw new NotImplementedException();
    }
}
