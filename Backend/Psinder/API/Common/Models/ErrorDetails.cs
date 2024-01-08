namespace Psinder.API.Common.Models;

public class ErrorDetails
{
    public ErrorDetails(string code, string? message = default, string? debug = default)
    {
        Code = code;
        Message = message;
        Debug = debug;
    }

    public string Code { get; set; }
    public string? Message { get; set; }
    public string? Debug { get; set; }
}