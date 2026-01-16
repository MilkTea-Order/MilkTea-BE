namespace MilkTea.Domain.Constants.Errors
{
    public class ErrorCode
    {
        #region General Errors (E0001-E0009)
        
        /// <summary>
        /// Not exists or not matching with data
        /// </summary>
        public const string E0001 = "E0001";

        /// <summary>
        /// Already exists
        /// </summary>
        public const string E0002 = "E0002";

        /// <summary>
        /// Required one of these items: Code or Email or Phone
        /// </summary>
        public const string E0004 = "E0004";

        /// <summary>
        /// Your account is not active. Please contact the administrator.
        /// </summary>
        public const string E0005 = "E0005";

        #endregion

        #region Validation Errors (E0010-E0019)

        /// <summary>
        /// Password matches the last "X" times.
        /// Frontend should get "X" to show to customer as password policy
        /// </summary>
        public const string E0012 = "E0012";

        #endregion

        #region Business Logic Errors (E0020-E0029)

        /// <summary>
        /// Processing error - General processing failure
        /// </summary>
        public const string E0027 = "E0027";

        /// <summary>
        /// Status invalid - The provided status is not valid for this operation
        /// </summary>
        public const string E0029 = "E0029";

        #endregion

        #region Data Validation Errors (E0030-E0039)

        /// <summary>
        /// Data is invalid - General data validation failure
        /// </summary>
        public const string E0036 = "E0036";

        #endregion

        #region Order/Product Errors (E0040-E0049)

        /// <summary>
        /// Item not available - The requested menu item is not available
        /// </summary>
        public const string E0040 = "E0040";

        /// <summary>
        /// Insufficient material stock - Not enough stock to fulfill the order
        /// </summary>
        public const string E0041 = "E0041";

        /// <summary>
        /// Cannot cancel order - Order status does not allow cancellation
        /// </summary>
        public const string E0042 = "E0042";

        #endregion

        #region Authentication Errors (E0043-E0049)

        /// <summary>
        /// Unauthorized - Token has been expired
        /// </summary>
        public const string E0043 = "E0043";

        /// <summary>
        /// Token invalid - Token has been revoked, not found, or invalid.
        /// </summary>
        public const string E0044 = "E0044";

        #endregion

        #region System Errors

        /// <summary>
        /// Internal server error - Unexpected system error
        /// </summary>
        public const string E9999 = "E9999";

        #endregion
    }
}
