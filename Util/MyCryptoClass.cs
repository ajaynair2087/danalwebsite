using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DanalJSONApp.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace DanalJSONApp.Util
{
    public static class MyCryptoClass
    {
        public static String AES_decrypt(String Input, String AES_Key, String AES_IV)
        {
            // Create decryptor
            RijndaelManaged aes = new RijndaelManaged();
            aes.KeySize = 128;
            aes.BlockSize = 128;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = Convert.FromBase64String(AES_Key);
            aes.IV = Convert.FromBase64String(AES_IV);
            var decrypt = aes.CreateDecryptor();

            // Decrypt Input
            byte[] xBuff = null;
            using (var ms = new MemoryStream())
            {
                // Convert from base64 string to byte array, write to memory stream and decrypt, then convert to byte array.
                using (var cs = new CryptoStream(ms, decrypt, CryptoStreamMode.Write))
                {
                    byte[] xXml = Convert.FromBase64String(Input);
                    cs.Write(xXml, 0, xXml.Length);
                }
                xBuff = ms.ToArray();
            }

            // Convert from byte array to UTF-8 string then return
            String Output = Encoding.UTF8.GetString(xBuff);
            return Output;
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }


        /// <summary>
        /// Encode the base64 to safeUrl64
        /// </summary>
        /// <param name="base64Str"></param>
        /// <returns></returns>
        public static string EncodeBase64Url(this string base64Str)
        {
            string Plus = "+";
            string Minus = "-";
            string Slash = "/";
            string Underscore = "_";
            string EqualSign = "=";
            string Pipe = "|";
            IDictionary<string, string> _mapper = new Dictionary<string, string> { { Plus, Minus }, { Slash, Underscore }, { EqualSign, Pipe } };
            if (string.IsNullOrEmpty(base64Str))
                return base64Str;

            foreach (var pair in _mapper)
            {
                base64Str = base64Str.Replace(pair.Key, pair.Value);
            }

            return base64Str;
        }
    }
}