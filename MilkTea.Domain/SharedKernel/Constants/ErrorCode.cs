namespace MilkTea.Domain.SharedKernel.Constants;

/// <summary>
/// Common error codes used across the domain.
/// </summary>
public static class ErrorCode
{
    // Generic error codes
    public const string NotFound = "NOT_FOUND";
    public const string InvalidOperation = "INVALID_OPERATION";
    public const string ValidationFailed = "VALIDATION_FAILED";
    public const string Unauthorized = "UNAUTHORIZED";
    public const string Forbidden = "FORBIDDEN";
    public const string Conflict = "CONFLICT";

    /// <summary>
    /// Not found / Not exist
    /// </summary>
    public const string E0001 = "E0001";
    /// <summary>
    /// Already exists / Duplicate
    /// </summary>
    public const string E0002 = "E0002";

    public const string E0004 = "E0004"; // Invalid / Empty
    public const string E0005 = "E0005"; // Invalid status
    public const string E0027 = "E0027"; // Operation failed
    public const string E0029 = "E0029"; // Invalid data
    public const string E0036 = "E0036"; // Invalid data / Validation failed
    public const string E0040 = "E0040"; // Not available
    public const string E0041 = "E0041"; // Insufficient stock
    /// <summary>
    /// Invalid status / Cannot perform operation
    /// </summary>
    public const string E0042 = "E0042";
    public const string E0012 = "E0012"; // Password reuse not allowed
    public const string E9999 = "E9999"; // Internal error
}
