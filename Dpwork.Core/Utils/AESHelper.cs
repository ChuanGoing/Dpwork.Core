using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Dpwork.Core.Utils
{
    public class AESHelper
    {
        private static readonly string _key = "Study@Dpwork.Core";

        public static string AesEncrypt(string str)
        {
            byte[] xBuff = null;

            try
            {
                RijndaelManaged aes = new RijndaelManaged
                {
                    KeySize = 128,
                    BlockSize = 128,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7,
                    Key = GetSecretKey(_key),
                    IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                };

                var encrypt = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encrypt, CryptoStreamMode.Write))
                    {
                        byte[] xXml = Encoding.UTF8.GetBytes(str);
                        cs.Write(xXml, 0, xXml.Length);
                        cs.FlushFinalBlock();
                    }

                    xBuff = ms.ToArray();
                }
            }
            catch
            {
                return null;
            }
            return ToHexString(xBuff);
        }

        public static string AesDecrypt(string str)
        {
            if (string.IsNullOrEmpty(str)) return null;

            string result;

            try
            {
                RijndaelManaged aes = new RijndaelManaged
                {
                    KeySize = 128,
                    BlockSize = 128,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7,
                    Key = GetSecretKey(_key),
                    IV = new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
                };

                var decrypt = aes.CreateDecryptor();
                byte[] encryptedStr = FromHex2ByteArray(str);


                using (var ms = new MemoryStream(encryptedStr))
                {
                    using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch
            {
                return null;
            }
            return result;
        }

        static byte[] GetSecretKey(string skey)
        {

            byte[] key = Encoding.UTF8.GetBytes(skey);
            byte[] k = new byte[16] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            for (int i = 0; i < key.Length; i++)
            {
                k[i % 16] = (byte)(k[i % 16] ^ key[i]);
            }

            return k;
        }

        static string ToHexString(byte[] ba)
        {
            return BitConverter.ToString(ba).Replace("-", "");
        }

        static byte[] FromHex2ByteArray(string hex)
        {
            if (hex.Length % 2 == 1)
                throw new Exception("The binary key cannot have an odd number of digits");

            byte[] arr = new byte[hex.Length >> 1];

            for (int i = 0; i < hex.Length >> 1; ++i)
            {
                arr[i] = (byte)((GetHexVal(hex[i << 1]) << 4) + (GetHexVal(hex[(i << 1) + 1])));
            }

            return arr;
        }

        static int GetHexVal(char hex)
        {
            int val = (int)hex;
            //For uppercase A-F letters:
            //return val - (val < 58 ? 48 : 55);
            //For lowercase a-f letters:
            //return val - (val < 58 ? 48 : 87);
            //Or the two combined, but a bit slower:
            return val - (val < 58 ? 48 : (val < 97 ? 55 : 87));
        }
    }
}
