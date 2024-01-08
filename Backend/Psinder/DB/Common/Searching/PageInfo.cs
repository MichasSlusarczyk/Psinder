namespace Psinder.DB.Common.Searching;

public class PageInfo
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public int Offset => (Page - 1) * PageSize;

    public static PageInfo Default => new PageInfo
    {
        Page = 1,
        PageSize = 50
    };

    public static PageInfo? FromQuery(int? page, int? pageSize)
    {
        if (page < 0 || page == null || pageSize < 0 || pageSize == null)
        {
            return null;
        }

        return new PageInfo()
        {
            Page = page.Value,
            PageSize = pageSize.Value
        };
    }
}
