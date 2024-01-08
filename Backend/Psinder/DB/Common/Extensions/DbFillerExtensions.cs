namespace Psinder.DB.Common.Extensions;

public static class DbFillerExtensions
{
    public static string ToInsertString(this int? value)
        => value is null ? "NULL" : $"{value}";

    public static string ToInsertString(this long? value)
        => value is null ? "NULL" : $"{value}";

    public static string ToInsertString(this string? value)
        => value is null ? "NULL" : $"'{value}'";

    public static string ToInsertString(this char? value)
        => value is null ? "NULL" : $"'{value}'";

    public static string ToInsertString(this bool? value)
        => value is null ? "NULL" : $"{value}";

    public static string CharToInsertString(this Enum? value)
        => value is null ? "NULL" : $"'{Convert.ToChar(value)}'";

    public static string IntToInsertString(this Enum? value)
        => value is null ? "NULL" : $"{Convert.ToInt32(value)}";
}
