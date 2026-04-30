using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MilkTea.Shared.Utils.Hash
{
    public class RSAHelper
    {
        public static string Decrypt(string CallBy, string PemPrivateKey, string CypherText)
        {
            string callFrom = CallBy + " -> " + System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType + " -> " + System.Reflection.MethodBase.GetCurrentMethod()?.Name;
            try
            {
                Org.BouncyCastle.OpenSsl.PemReader pr = new Org.BouncyCastle.OpenSsl.PemReader(new StringReader(PemPrivateKey));
                AsymmetricCipherKeyPair KeyPair = (AsymmetricCipherKeyPair)pr.ReadObject();
                RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)KeyPair.Private);

                RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                csp.ImportParameters(rsaParams);

                var resultBytes = Convert.FromBase64String(CypherText);
                var decryptedBytes = csp.Decrypt(resultBytes, true);
                var decryptedData = Encoding.UTF8.GetString(decryptedBytes);

                return decryptedData.ToString();
            }
            catch (Exception ex)
            {
                LogHelper.Write(callFrom + ":\n" + ex.Message);
                return "";
            }
        }

        public static string Encrypt(string CallBy, string PemPublicKey, string PlainText)
        {
            string callFrom = CallBy + " -> " + System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType + " -> " + System.Reflection.MethodBase.GetCurrentMethod()?.Name;
            try
            {
                Org.BouncyCastle.OpenSsl.PemReader pr = new Org.BouncyCastle.OpenSsl.PemReader(new StringReader(PemPublicKey));
                AsymmetricKeyParameter publicKey = (AsymmetricKeyParameter)pr.ReadObject();
                RSAParameters rsaParams = DotNetUtilities.ToRSAParameters((RsaKeyParameters)publicKey);

                RSACryptoServiceProvider csp = new RSACryptoServiceProvider();
                csp.ImportParameters(rsaParams);

                var data = Encoding.UTF8.GetBytes(PlainText);
                var encryptedData = csp.Encrypt(data, true);
                var base64Encrypted = Convert.ToBase64String(encryptedData);
                return base64Encrypted.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                LogHelper.Write(callFrom + ":\n" + ex.Message);
                return "";
            }
        }

        private static int GetPositionCharacter(char c)
        {
            switch (c)
            {
                case 'a':
                    return 8;
                case 'b':
                    return 16;
                case 'c':
                    return 10;
                case 'd':
                    return 21;
                case 'e':
                    return 15;
                case 'f':
                    return 7;
                case 'g':
                    return 20;
                case 'h':
                    return 25;
                case 'i':
                    return 23;
                case 'j':
                    return 14;
                case 'k':
                    return 3;
                case 'l':
                    return 6;
                case 'm':
                    return 11;
                case 'n':
                    return 26;
                case 'o':
                    return 22;
                case 'p':
                    return 1;
                case 'q':
                    return 17;
                case 'r':
                    return 13;
                case 's':
                    return 9;
                case 't':
                    return 4;
                case 'u':
                    return 2;
                case 'v':
                    return 18;
                case 'w':
                    return 5;
                case 'x':
                    return 24;
                case 'y':
                    return 19;
                case 'z':
                    return 12;
                default:
                    return 0;
            }
        }

        public static string RemoveCharactersInKey(string CallBy, string PemKey, bool IsPublicKey)
        {
            string callFrom = CallBy + " -> " + System.Reflection.MethodBase.GetCurrentMethod()?.DeclaringType + " -> " + System.Reflection.MethodBase.GetCurrentMethod()?.Name;
            try
            {
                //Cắt chuỗi key
                string[] separatingStrings = { "\r\n", "\n" };
                string[] arrKey = PemKey.Split(separatingStrings, System.StringSplitOptions.RemoveEmptyEntries);

                //Convert sang List cho dễ xử lý
                List<string> lstKey = new List<string>(arrKey);

                //Lấy chuỗi kí tự random, bỏ 1 dòng đầu và 2 dòng cuối -> -3
                int characterQty = lstKey.Count - 3;
                string characterString = lstKey[lstKey.Count - 2].Substring(0, characterQty);

                //Lấy List kí tự và vị trí tương ứng
                List<int> lstCharPos = new List<int>();
                foreach (char c in characterString)
                    lstCharPos.Add(GetPositionCharacter(c));

                //Xóa kí tự dòng key số 1: 
                if (IsPublicKey)
                    lstKey[1] = lstKey[1].Remove(lstCharPos[0] + 44, 1);//vị trí cộng thêm 44 -> vì dòng này luôn bắt đầu là chuỗi dài 44
                else
                    lstKey[1] = lstKey[1].Remove(lstCharPos[0] + 4, 1);//vị trí cộng thêm 4 -> vì dòng này luôn bắt đầu là MIIE

                //Xóa các kí tự tiếp theo với vị trí tương ứng của từng kí tự
                for (int i = 1; i < lstCharPos.Count; i++)
                    lstKey[1 + i] = lstKey[1 + i].Remove(lstCharPos[i], 1);

                //Xóa chuỗi kí tự
                lstKey[lstKey.Count - 2] = lstKey[lstKey.Count - 2].Substring(characterQty);

                //Nối các key lại
                string keyReturn = "";
                for (int i = 0; i < lstKey.Count; i++)
                {
                    if (i == lstKey.Count - 1)
                        keyReturn = keyReturn + lstKey[i];
                    else
                        keyReturn = keyReturn + lstKey[i] + "\r\n";
                }

                return keyReturn;
            }
            catch (Exception ex)
            {
                LogHelper.Write(callFrom + ":\n" + ex.Message);
                return "";
            }
        }

    }
}
