namespace Psinder.API.Common.Exceptions;

public class OperationFailedException : Exception
{
    public const string CODE = "OPERATION_FAILED";
    public const string DEFAULT_MESSAGE = "The attempted operation failed.";

    public OperationFailedException(string? message = default) : base(message ?? DEFAULT_MESSAGE) { }
}