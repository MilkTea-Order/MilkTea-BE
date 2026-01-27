namespace MilkTea.Application.Common.Exceptions;

public class RequestValidationException : Exception
{
    public IReadOnlyDictionary<string, object> ErrorData { get; }
    public string? PrimaryErrorCode { get; }

    public RequestValidationException(
        string message,
        IReadOnlyDictionary<string, object> errorData,
        string? primaryErrorCode = null)
        : base(message)
    {
        ErrorData = errorData;
        PrimaryErrorCode = primaryErrorCode;
    }
}

