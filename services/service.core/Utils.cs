using System.Text;
using System.Security.Cryptography;
using System;

namespace service.core
{
    public class Utils
    {
        public static string MD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var inputBytes = Encoding.Default.GetBytes(input);
                var result = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder(32);

                foreach (var b in result)
                {
                    sb.Append(b.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}