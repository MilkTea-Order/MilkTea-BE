namespace MilkTea.Domain.Constants.Errors
{
    public class ErrorCode
    {
        /// <summary>
        /// Not exists or not matching with data
        /// </summary>
        public const string E0001 = "E0001";

        /// <summary>
        /// Already exists
        /// </summary>
        public const string E0002 = "E0002";

        /// <summary>
        /// Required all items: Code and Email and Phone, 
        /// Ex: "E0003" = ["Code", "Email", "Phone"]
        /// </summary>
        public const string E0003 = "E0003";

        /// <summary>
        /// Required one of these items: Code or Email or Phone,
        /// Ex: "E0004" = ["Code", "Email", "Phone"]
        /// </summary>
        public const string E0004 = "E0004";

        /// <summary>
        /// Your account is not active. Please contact the administrator.
        /// </summary>
        public const string E0005 = "E0005";
        /// <summary>
        /// Date not in format YYYY-MM-DD
        /// </summary>
        public const string E0006 = "E0006";
        /// <summary>
        /// Your account is not verified yet. Please verify your email and phone.
        /// </summary>
        public const string E0007 = "E0007";
        /// <summary>
        /// Cannot update data
        /// </summary>
        public const string E0008 = "E0008";

        /// <summary>
        /// Already verified, cannot update
        /// </summary>
        public const string E0009 = "E0009";

        /// <summary>
        /// Otp Code is expired
        /// </summary>
        public const string E0010 = "E0010";

        /// <summary>
        /// Otp Code reach maximum number of attempts
        /// </summary>
        public const string E0011 = "E0011";

        /// <summary>
        /// Password matches the last "X" times,
        /// FE have to get "X" to show to customer, this will show as password policy
        /// </summary>
        public const string E0012 = "E0012";

        /// <summary>
        /// File extension is not allowed (jpg/jpeg/png)
        /// </summary>
        public const string E0013 = "E0013";

        /// <summary>
        /// Cannot open sftp setting
        /// </summary>
        public const string E0014 = "E0014";
        /// <summary>
        /// Sftp's directory not exists
        /// </summary>
        public const string E0015 = "E0015";
        /// <summary>
        /// Cannot store file in sftp
        /// </summary>
        public const string E0016 = "E0016";
        /// <summary>
        /// File size is not allow (20K ~ 5M)
        /// </summary>
        public const string E0017 = "E0017";
        /// <summary>
        /// File resolution not allow 50x50 px (min), 5000x5000 px (max)
        /// </summary>
        public const string E0018 = "E0018";
        /// <summary>
        /// Date not in format DD/MM/YYYY
        /// </summary>
        public const string E0019 = "E0019";
        /// <summary>
        /// Data type is not an int
        /// </summary>
        public const string E0020 = "E0020";
        /// <summary>
        /// Data exceeds allowed length, Ex: "Rows[1] Columns[1:10, 2:30]", 
        /// means Row 1, column 1 's max length is 10, column 2 's max length is 20
        /// </summary>
        public const string E0021 = "E0021";
        /// <summary>
        /// Serial duplicate,
        /// serial_number1: Rows[1,3,5], serial_number2: Rows[6,7,8]
        /// </summary>
        public const string E0022 = "E0022";

        /// <summary>
        /// File extension is not allowed (xlsx/xls)
        /// </summary>
        public const string E0023 = "E0023";

        /// <summary>
        /// Duplicate Value,
        /// Ex [A, B, C, B, D, C] (this list can be request data or data in db), will return [B, C]
        /// </summary>
        public const string E0024 = "E0024";

        /// <summary>
        /// File extension is not allowed (JPG/JPEG/PNG)
        /// </summary>
        public const string E0025 = "E0025";
        /// <summary>
        /// File size is not allow (Max 5M)
        /// </summary>
        public const string E0026 = "E0026";

        /// <summary>
        /// Processing error
        /// </summary>
        public const string E0027 = "E0027";
        /// <summary>
        /// Cannot create sftp folder
        /// </summary>
        public const string E0028 = "E0028";

        /// <summary>
        ///  status invalid
        /// </summary>
        public const string E0029 = "E0029";

        /// <summary>
        /// PurchasedDate is invalid
        /// </summary>
        public const string E0030 = "E0030";

        /// <summary>
        /// Limit total file can upload, Ex: dict[E0031] = 3, means total file can upload is 3,
        /// </summary>
        public const string E0031 = "E0031";

        /// <summary>
        /// Date is after current date (must less than or equal to current date)
        /// </summary>
        public const string E0032 = "E0032";

        /// <summary>
        /// Dữ liệu Email/Phone lỗi. Có " + {count} + " tài khoản cùng Email/Phone,
        /// {count} is value of this error
        /// </summary>
        public const string E0033 = "E0033";
        /// <summary>
        /// Trạng thái tài khoản đang tạm ngưng. Vui lòng liên hệ Admin để được tiếp tục sử dụng dịch vụ.
        /// </summary>
        public const string E0034 = "E0034";

        /// <summary>
        /// GroupID is invalid
        /// </summary>
        public const string E0035 = "E0035";

        /// <summary>
        /// Data is invalid
        /// </summary>
        public const string E0036 = "E0036";


        /// <summary>
        /// Item not available
        /// </summary>
        public const string E0040 = "E0041";
        /// <summary>
        /// // Insufficient material stock
        /// </summary>
        public const string E0041 = "E0040";

        /// <summary>
        /// Cannot cancel order - order status does not allow cancellation
        /// </summary>
        public const string E0042 = "E0042";

        public const string E9999 = "E9999";
    }
}
