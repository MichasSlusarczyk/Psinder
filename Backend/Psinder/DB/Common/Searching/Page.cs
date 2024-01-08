namespace Psinder.DB.Common.Searching;

public class Page<T>
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalSize { get; set; }

    public List<T> List { get; set; }
}
