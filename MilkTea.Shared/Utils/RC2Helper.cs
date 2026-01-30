using System.Security.Cryptography;
using System.Text;
namespace Shared.Utils
{
    public class RC2Helper
    {
        public static string CreateHashValue(String plaintext, int n)
        {
            byte[] buf, result;
            buf = Encoding.ASCII.GetBytes(plaintext);
            using (var hash = SHA256.Create())
            {
                result = hash.ComputeHash(buf);
            }
            return BitConverter.ToString(result).Substring(0, n);
        }
        public static string EncryptByRC2(String plaintext, String Key, String IV)
        {


            int size = plaintext.Length;
            //tạo 1 thể hiện cho RC2
            using (var myRC2 = System.Security.Cryptography.RC2.Create())
            {
                //tạo khóa cho RC2
                myRC2.Key = Encoding.ASCII.GetBytes(CreateHashValue(Key, 16));
                myRC2.IV = Encoding.ASCII.GetBytes(CreateHashValue(IV, 8));

                //tạo một bộ mã hóa
                ICryptoTransform myRC2_Ecryptor = myRC2.CreateEncryptor(myRC2.Key, myRC2.IV);

                //dữ liệu muốn mã hóa được đưa vào một vùng nhớ
                MemoryStream memEncrypt = new MemoryStream();
                //tạo 1 crypto stream
                CryptoStream EncryptCrypto = new CryptoStream(memEncrypt, myRC2_Ecryptor, CryptoStreamMode.Write);

                //đọc dữ liệu vào 1 mảng byte
                byte[] arrEncript = Encoding.ASCII.GetBytes(plaintext);

                //ghi tất cả dữ liệu mã hóa xuống crypto stream
                EncryptCrypto.Write(arrEncript, 0, arrEncript.Length);
                EncryptCrypto.FlushFinalBlock();

                //lấy dữ liệu đã mã hóa ra tử vùng nhớ byte[] = memEncrypt.ToArray()
                byte[] arrEncripted = memEncrypt.ToArray();

                String kq = plaintext.Length + " " + Convert.ToBase64String(arrEncripted, 0, arrEncripted.Length, Base64FormattingOptions.InsertLineBreaks);

                memEncrypt.Close();
                EncryptCrypto.Close();
                return kq;
            }
        }
        public static string DecryptByRC2(String encryptedtext, String Key, String IV)
        {

            //lay kich thuoc
            int i = 0;
            while (encryptedtext[i] != ' ')
            {
                i++;
            }
            String str = encryptedtext.Substring(0, i);
            int length = Int32.Parse(str);

            //đọc dữ liệu da ma hoa vào 1 mảng byte
            byte[] arrEncripted = Convert.FromBase64String(encryptedtext.Remove(0, i + 1));

            //tạo 1 thể hiện cho RC2
            using (var myRC2 = System.Security.Cryptography.RC2.Create())
            {
                //tạo khóa cho RC2
                myRC2.Key = Encoding.ASCII.GetBytes(CreateHashValue(Key, 16));
                myRC2.IV = Encoding.ASCII.GetBytes(CreateHashValue(IV, 8));

                //tạo một bộ mã hóa
                ICryptoTransform myRC2_Decryptor = myRC2.CreateDecryptor(myRC2.Key, myRC2.IV);

                //dữ liệu muốn giải mã được đưa vào một vùng nhớ
                MemoryStream memDecrypt = new MemoryStream(arrEncripted);
                //tạo 1 crypto stream
                CryptoStream DecryptCrypto = new CryptoStream(memDecrypt, myRC2_Decryptor, CryptoStreamMode.Read);

                //lấy lại dữ liệu từ crypto stream --> byte[]
                byte[] arrDecripted = new byte[length];
                DecryptCrypto.Read(arrDecripted, 0, arrDecripted.Length);

                //String kq = Convert.ToBase64String(arrDecripted,0,arrDecripted.Length, Base64FormattingOptions.InsertLineBreaks);
                String kq = Encoding.ASCII.GetString(arrDecripted);

                //dong cac stream
                DecryptCrypto.Close();
                memDecrypt.Close();
                return kq;
            }
        }
    }
}
