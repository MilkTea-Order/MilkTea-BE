namespace MilkTea.Shared.Utils
{
    public class LogHelper
    {
        private static void WriteDate(ref string DateLog, ref string FileName)
        {
            var now = DateTime.Now;
            string year = now.Year.ToString();
            string month = now.Month.ToString("00");
            string day = now.Day.ToString("00");
            string hour = now.Hour.ToString("00");
            string minute = now.Minute.ToString("00");
            string second = now.Second.ToString("00");
            DateLog = day + "-" + month + "-" + year + " " + hour + ":" + minute + ":" + second;
            FileName = year + "-" + month + "-" + day + ".txt";
        }

        public static void Write(string Content)
        {
            string vLogErrorPath = Directory.GetCurrentDirectory() + "\\LogError";
            if (!Directory.Exists(vLogErrorPath))
                Directory.CreateDirectory(vLogErrorPath);

            string dateLog = "", fileName = "";
            WriteDate(ref dateLog, ref fileName);

            FileInfo fInfo = new FileInfo(vLogErrorPath + "\\" + fileName);
            StreamWriter text = fInfo.AppendText();
            text.WriteLine("\n" + dateLog + "\n" + Content);
            text.Close();
        }
    }
}
