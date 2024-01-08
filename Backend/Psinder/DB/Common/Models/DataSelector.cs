namespace Psinder.DB.Common.Models;

public class DataSelector
{
    public string? FilterValue { get; set; }
    public string? OrderColumn { get; set; }
    public OrderDirections OrderDirection { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}
