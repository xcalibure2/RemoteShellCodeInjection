﻿using System.Security.Cryptography;
using System.Text;

namespace RemoteShellCodeInjection.Utils
{
    public static class Utils
    {
        public static string FromHexString(string hexString)
        {
            byte[]? bytes = new byte[hexString.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return Encoding.UTF8.GetString(bytes);
        }


        //totaly stolen from : https://github.com/san3ncrypt3d/AESShellCodeInjector/blob/main/AESInject/AESInject/Program.cs
        public static byte[] AESDecrypt(byte[] CEncryptedShell, byte[] key, byte[] iv)
        {
            using (Aes? aes = Aes.Create())
            {
                aes.KeySize = 128;
                aes.BlockSize = 128;
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.CBC;
                aes.Key = key;
                aes.IV = iv;

                using (ICryptoTransform? decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    return GetDecrypt(CEncryptedShell, decryptor);
                }
            }
        }
        private static byte[] GetDecrypt(byte[] data, ICryptoTransform cryptoTransform)
        {
            using (MemoryStream? ms = new MemoryStream())
            using (CryptoStream? cryptoStream = new CryptoStream(ms, cryptoTransform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                return ms.ToArray();
            }
        }
    }
}
