namespace MilkTea.Shared.Templates
{
    public class OtpEmail
    {
        public static string BuildOtpTemplate(
            string otpCode,
            int expireMinutes,
            string title = "Mã OTP",
            string companyName = "My Company")
        {
            var html = $@"
<!DOCTYPE html>
<html lang=""vi"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>{title}</title>
</head>

<body style=""margin:0;padding:0;background-color:#f4f6f8;font-family:Arial,Helvetica,sans-serif;"">
    <table width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#f4f6f8;padding:24px 0;"">
        <tr>
            <td align=""center"">
                <table width=""600"" cellpadding=""0"" cellspacing=""0"" style=""background:#ffffff;border-radius:12px;overflow:hidden;"">

                    <!-- Header -->
                    <tr>
                        <td style=""background:#2563eb;padding:20px;text-align:center;color:#ffffff;font-size:24px;font-weight:bold;"">
                            {companyName}
                        </td>
                    </tr>

                    <!-- Content -->
                    <tr>
                        <td style=""padding:32px;"">

                            <p style=""margin:0 0 16px;color:#374151;font-size:16px;line-height:1.6;"">
                                {title}
                            </p>

                            <div style=""text-align:center;margin:24px 0;"">
                                <span style=""display:inline-block;padding:16px 32px;font-size:32px;font-weight:bold;letter-spacing:8px;color:#2563eb;background:#eff6ff;border:2px dashed #93c5fd;border-radius:10px;"">
                                    {otpCode}
                                </span>
                            </div>

                            <p style=""margin:0 0 16px;color:#374151;font-size:15px;line-height:1.6;"">
                                Mã này sẽ hết hạn sau <strong>{expireMinutes} phút</strong>.
                            </p>

                            <p style=""margin:0 0 16px;color:#374151;font-size:15px;line-height:1.6;"">
                                Nếu bạn không yêu cầu mã này, vui lòng bỏ qua email này.
                            </p>

                            <hr style=""border:none;border-top:1px solid #e5e7eb;margin:24px 0;"" />

                            <p style=""margin:0;color:#6b7280;font-size:13px;line-height:1.6;text-align:right;"">
                                Trân trọng,<br/>
                                <strong>{companyName}</strong>
                            </p>

                        </td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
</body>
</html>";

            return html;
        }
    }
}
