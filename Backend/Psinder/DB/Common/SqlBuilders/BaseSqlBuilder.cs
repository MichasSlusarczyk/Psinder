using Dapper;
using Psinder.DB.Common.Searching;

namespace Psinder.DB.Common.SqlBuilders;

public class BaseSqlBuilder
{
    protected SqlBuilder _sqlBuilder = new();

    protected string _pagingPart = string.Empty;

    protected string _fromPart = string.Empty;

    protected string _totalSizeSelectPartPart = string.Empty;

    protected void CreatePagingPart(PageInfo pageInfo)
    {
        _pagingPart = string.Format(
            @"LIMIT @limitValue {0}", pageInfo.Offset > 0 ? "OFFSET @offset" : string.Empty);
    }

    protected void AddSorting<TSortEnum>(SortInfo<TSortEnum>? sorting) where TSortEnum : Enum
    {
        if (string.IsNullOrEmpty(sorting?.ColumnName))
        {
            return;
        }

        _sqlBuilder.OrderBy(string.Format("{0} {1}", sorting.ColumnName, sorting.Direction.ToString()));
    }

    protected void AddPagingParameters(PageInfo paging)
    {
        _sqlBuilder.AddParameters(new { limitValue = paging.PageSize });
        if(paging.Offset > 0)
        {
            _sqlBuilder.AddParameters(new { offset = paging.Offset });
        }
    }

    protected void CreateFromPart(string mainTableName)
    {
        _fromPart = $"FROM {mainTableName}";
    }

    protected void CreateTotalSizeSelectPart(string mainColumnName)
    {
        _totalSizeSelectPartPart = $"SELECT COUNT({mainColumnName})";
    }
}
