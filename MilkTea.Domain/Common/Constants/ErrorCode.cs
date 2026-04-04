namespace MilkTea.Domain.Common.Constants;


public static class ErrorCode
{
    /// <summary>
    /// // Not found / Not exist
    /// </summary>
    public const string E0001 = "E0001";
    /// <summary>
    /// Already exists / Duplicate
    /// </summary>
    public const string E0002 = "E0002";

    /// <summary>
    /// Invalid / Empty
    /// </summary>
    public const string E0004 = "E0004";

    /// <summary>
    ///  Password reuse not allowed
    /// </summary>
    public const string E0012 = "E0012";

    /// <summary>
    /// // Operation failed
    /// </summary>
    public const string E0027 = "E0027";


    /// <summary>
    /// // Invalid data / Validation failed
    /// </summary>
    public const string E0036 = "E0036";

    /// <summary>
    ///  // Insufficient stock
    /// </summary>
    public const string E0041 = "E0041";
    /// <summary>
    /// Invalid status / Cannot perform operation
    /// </summary>
    public const string E0042 = "E0042";

    /// <summary>
    /// Internal error
    /// </summary>
    public const string E9999 = "E9999";

    //public const string E0040 = "E0040"; // Not available
}
