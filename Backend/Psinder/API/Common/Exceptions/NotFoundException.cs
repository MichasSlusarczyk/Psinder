namespace Psinder.API.Common.Exceptions;

public class NotFoundException : Exception
{
    public const string CODE = "NOT_FOUND";
    public const string DEFAULT_MESSAGE = "Resource was not found.";

    public NotFoundException(string? message = default) : base(message ?? DEFAULT_MESSAGE) { }
}
