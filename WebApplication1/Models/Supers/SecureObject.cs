using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace WebApplication1.Models
{
    using System.Security.Cryptography;
    using System.Text;

    public class SecureObject
    {

        /**
        * <para>Sanitizse strings by
        * decoding html script to its escaped form, and
        * escaping apostrophes and double quotes to prevent
        * SQL injections and XSS Attacks.</para>
        *
        * <returns>a sanitized string</returns>
        */
        public string Sanitize(string s)
        {
            s = Regex.Replace(WebUtility.HtmlDecode(s), @"\s+", " ").Trim();
            s = s.Replace("'", "\'").Replace(@"""", "\"");

            return s;
        }

        /**
         * <para> Adds salt to the given string before calling <see cref="Hash(string)"/> on it</para>
         *
         * <returns> a hash computed string</returns>
         */
        public string Hash(string s, string salt)
        {
            return Hash(s + salt);
        }

        /**
         * <para>Hashes a string using the SHA256 algorithm</para>
         *
         * <returns> a hash computed string </returns>
         */
        public string Hash(string s)
        {
            using (var sha256 = new SHA256Managed())
            {
                byte[] raw = Encoding.Default.GetBytes(s);
                StringBuilder sb = new StringBuilder();

                foreach (byte c in raw)
                {
                    sb.Append(c.ToString("x2"));
                }

                s = sb.ToString();
            }
            return s;
        }

    }
}