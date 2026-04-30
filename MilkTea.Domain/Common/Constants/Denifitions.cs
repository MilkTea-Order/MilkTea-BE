namespace MilkTea.Shared.Domain.Constants
{
    public static class Denifinitions
    {

        public const string BILL_PREFIX_CODE = "BILL_PREFIX";
        public const string WAREHOUSE_MANAGEMENT_MODE_CODE = "WAREHOUSE";
        public const string COLLECT_TYPE = "Tiền thu";

        // OTP Constants
        public const string TIME_EXPIRED_OTP_CODE = "TIME_EXPIRED_OTP";
        public const string MAX_NUMBER_OF_TIMES_OTP = "MAX_NUMBER_OF_TIMES_OTP";
        // Token
        public const string TIME_EXPIRED_TOKEN_CODE = "TIME_EXPIRED_TOKEN";
        // Session
        public const string TIME_EXPIRED_SESSION_CODE = "TIME_EXPIRED_SESSION";
        public const string VERIFY_INTERVAL_MINUTES_CODE = "VERIFY_INTERVAL_MINUTES";

        // OTP Resend
        public const string OTP_RESEND_MAX_ATTEMPTS_CODE = "OTP_RESEND_MAX_ATTEMPTS";

        // OTP Send Limit per Channel
        public const string OTP_EMAIL_COUNT_CODE = "OTP_EMAIL_COUNT";
        public const string OTP_SMS_COUNT_CODE = "OTP_SMS_COUNT";

        public const string OTP_TYPE_FORGOT_PASSWORD = "FORGOT_PASSWORD";
        public const string OTP_TYPE_CHANGE_PASSWORD = "CHANGE_PASSWORD";
    }
}
