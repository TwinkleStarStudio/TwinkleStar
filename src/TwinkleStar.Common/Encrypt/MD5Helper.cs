using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TwinkleStar.Common
{
    /// <summary>
    /// MD5工具类
    /// </summary>
    public class MD5Helper
    {

        public static string Encrypt(string text, string salt)
        {
            text += salt;

            return Encrypt(text);
        }

        public static string Encrypt(string text)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            var md5 = MD5.Create();
            byte[] cipherData = md5.ComputeHash(data);

            return Convert.ToBase64String(cipherData);
        }

    }
}
