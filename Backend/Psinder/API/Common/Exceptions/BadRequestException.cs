namespace Psinder.API.Common.Exceptions;

public class BadRequestException : Exception
{
    public const string CODE = "BAD_REQUEST";
    public const string DEFAULT_MESSAGE = "Bad request.";

    public BadRequestException(string? message = default) : base(message ?? DEFAULT_MESSAGE) { }
}
