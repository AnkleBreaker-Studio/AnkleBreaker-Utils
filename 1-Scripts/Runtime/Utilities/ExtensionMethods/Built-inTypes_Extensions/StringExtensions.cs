using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Security.Cryptography;
using System.Text;
#if NEWTONSOFT_JSON
using Newtonsoft.Json;
#endif

namespace AnkleBreaker.Utils.ExtensionMethods.BuiltIn_Types
{
    public static class StringExtensions
    {
        public static string ToTitleCase(this string input)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int index = 0; index < input.Length; ++index)
            {
                char ch = input[index];
                if (ch == '_' && index + 1 < input.Length)
                {
                    char upper = input[index + 1];
                    if (char.IsLower(upper))
                        upper = char.ToUpper(upper, CultureInfo.InvariantCulture);
                    stringBuilder.Append(upper);
                    ++index;
                }
                else
                    stringBuilder.Append(ch);
            }
            return stringBuilder.ToString();
        }
            
        public static string ToPascalCase(this string str)
        {
            if (string.IsNullOrEmpty(str)) return str;
            str = str.Replace(" ", "_").ToTitleCase();
            return str;
        }
        
        public static string ToCamelCase(this string str, bool isFirstLetterCamelCase = false)
        {
            str = str?.ToPascalCase();
            if (string.IsNullOrEmpty(str)) return str;

            char firstLetter = str[0];

            if (isFirstLetterCamelCase)
            {
                firstLetter = char.ToUpperInvariant(firstLetter);
            }
            else
            {
                firstLetter = char.ToLowerInvariant(firstLetter);
            }
            
            return firstLetter + str.Substring(1);
        }
        
        public static string Concat(this List<string> obj, string separator)
        {
            string result = "";

            int nbrOfElement = obj.Count;
            if (nbrOfElement > 0)
            {
                result = obj[0];
                for (int i = 1; i < nbrOfElement; i++)
                    result += separator + obj[i];
            }

            return result;
        }

        public static uint IPStringToUint(this string address)
        {
            var ipBytes = IPStringToBytes(address);
            var ip = (uint)ipBytes[0] << 24;
            ip += (uint)ipBytes[1] << 16;
            ip += (uint)ipBytes[2] << 8;
            ip += (uint)ipBytes[3];
            return ip;
        }

        public static byte[] IPStringToBytes(this string address)
        {
            var ipAddress = IPAddress.Parse(address);
            return ipAddress.GetAddressBytes();
        }

        public static string CalculateSHA256HashWithNonce(this string input, string nonce = "", Encoding encoding = null)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                if (encoding == null)
                    encoding = Encoding.UTF8;
                byte[] inputBytes = encoding.GetBytes(input + nonce);
                byte[] hashBytes = sha256.ComputeHash(inputBytes);

                StringBuilder stringBuilder = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                    stringBuilder.Append(hashBytes[i].ToString("X2")); // Convert byte to hexadecimal representation

                return stringBuilder.ToString();
            }
        }

#if NEWTONSOFT_JSON
        public static T DeserializeObjectFromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
#endif
    }
}
