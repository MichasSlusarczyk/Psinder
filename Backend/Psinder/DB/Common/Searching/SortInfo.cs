using Psinder.DB.Common.Extensions;

namespace Psinder.DB.Common.Searching;

public class SortInfo<TSortEnum> where TSortEnum : Enum
{
    public TSortEnum? ByColumn { get; set; }

    public string? ColumnName => ByColumn?.GetDescription();

    public SortDirections Direction { get; set; } = SortDirections.ASC;

    public string CreateSortExpression()
    {
        if(ByColumn == null)
        {
            return string.Empty;
        }

        return string.Format("{0} {1}", ByColumn.ToString(), Direction.ToString());
    }

    public static SortInfo<TSortEnum>? Parse(string? sortExpression)
    {
        if (string.IsNullOrEmpty(sortExpression)) 
        { 
            return null;
        }

        var values = sortExpression.Split(' ');

        return new SortInfo<TSortEnum>()
        {
            ByColumn = (TSortEnum)Enum.Parse(typeof(TSortEnum), values[0]),
            Direction = (SortDirections)Enum.Parse(typeof(SortDirections), values[1])
        };
    }

    public static SortInfo<TSortEnum> Create(TSortEnum byColumn, SortDirections direction)
    {
        return new SortInfo<TSortEnum>()
        {
            ByColumn = byColumn,
            Direction = direction
        };
    }
}
