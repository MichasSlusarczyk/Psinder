namespace Psinder.API.Common.Exceptions;

public class AccessDeniedException : Exception
{
    public const string CODE = "ACCESS_DENIED";
    public const string DEFAULT_MESSAGE = "Access to resource was denied.";

    public AccessDeniedException(string? message = default) : base(message ?? DEFAULT_MESSAGE) { }
}
