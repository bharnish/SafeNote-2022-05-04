using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication;

namespace SafeNote.WebAPI.Controllers
{
    public static class Utils
    {
        public static string Hash(byte[] data)
        {
            using var hash = SHA256.Create();

            return Encode(hash.ComputeHash(data));
        }

        public static string Hash(string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            return Hash(bytes);
        }

        public static string DecryptAES(string data, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            var bytes = Decode(data);

            using var ms = new MemoryStream(bytes);
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);
            return sr.ReadToEnd();
        }

        public static string EncryptAES(string data, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;

            using var ms = new MemoryStream();

            using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                using var sw = new StreamWriter(cs);
                sw.Write(data);
            }

            return Encode(ms.ToArray());
        }

        public static string Encode(byte[] data) => Base64UrlTextEncoder.Encode(data);
        public static byte[] Decode(string data) => Base64UrlTextEncoder.Decode(data);
    }
}