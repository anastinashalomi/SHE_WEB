using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.IO;

/// <summary>
/// Summary description for EncryptDecrypt
/// </summary>
namespace SHE.Code
{
    public class EncryptDecrypt
    {

        private const string ENCRYPTION_KEY = "J114&=a39sI+XyZR$#@123456789";
        private readonly static byte[] SALT = Encoding.ASCII.GetBytes(ENCRYPTION_KEY.Length.ToString());

        public EncryptDecrypt()
        {
        }

        public string Encrypt(string inputText)
        {
            try
            {
                RijndaelManaged rijndaelCipher = new RijndaelManaged();
                byte[] plainText = Encoding.Unicode.GetBytes(inputText);
                PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(ENCRYPTION_KEY, SALT);

                using (ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16)))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainText, 0, plainText.Length);
                            cryptoStream.FlushFinalBlock();
                            return Convert.ToBase64String(memoryStream.ToArray());
                        }
                    }
                }
            }
            catch
            {
                return "#";
            }
        }

        public string Decrypt(string inputText)
        {
            try
            {
                RijndaelManaged rijndaelCipher = new RijndaelManaged();


                byte[] encryptedData = Convert.FromBase64String(inputText.Replace(" ", "+"));
                PasswordDeriveBytes secretKey = new PasswordDeriveBytes(ENCRYPTION_KEY, SALT);

                using (ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16)))
                {
                    using (MemoryStream memoryStream = new MemoryStream(encryptedData))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            byte[] plainText = new byte[encryptedData.Length];
                            int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
                            return Encoding.Unicode.GetString(plainText, 0, decryptedCount);

                        }
                    }
                }
            }
            catch
            {
                return "#";
            }


        }




        public Dictionary<string, string> getParameters(string str)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();
            string[] valPairs = str.Split('&');
            foreach (string q in valPairs)
            {
                string[] valParaVal = q.Split('=');
                dict.Add(valParaVal[0], valParaVal[1]);
            }
            return dict;
        }

        public bool filter_IDs(string input)
        {
            bool result = true;
            //char[] badStuff = new char[] { '\'', '=', ';', '-' };  
            char[] badStuff = new char[] { '\'', '=', ';' };
            int i = 0;
            i = input.IndexOfAny(badStuff);

            if (input.Contains("--"))
            {
                result = false;
            }
            else
            {
                if (i != -1)
                {
                    result = false;
                }
            }
            return result;
        }

        private string htmldecode(string input)
        {
            string output = "";
            output = input;
            output = output.Replace("%20", "+");
            output = output.Replace("%21", "!");
            output = output.Replace("%22", "\"");
            output = output.Replace("%23", "#");
            output = output.Replace("%24", "$");
            output = output.Replace("%25", "%");
            output = output.Replace("%26", "&");
            output = output.Replace("%27", "\'");
            output = output.Replace("%28", "(");
            output = output.Replace("%29", ")");
            output = output.Replace("%2A", "*");
            output = output.Replace("%2B", "+");
            output = output.Replace("%2C", ",");
            output = output.Replace("%2D", "-");
            output = output.Replace("%2E", ".");
            output = output.Replace("%2F", "/");
            output = output.Replace("%2a", "*");
            output = output.Replace("%2b", "+");
            output = output.Replace("%2c", ",");
            output = output.Replace("%2d", "-");
            output = output.Replace("%2e", ".");
            output = output.Replace("%2f", "/");

            output = output.Replace("%3A", ":");
            output = output.Replace("%3B", ";");
            output = output.Replace("%3C", "<");
            output = output.Replace("%3D", "=");
            output = output.Replace("%3E", ">");
            output = output.Replace("%3F", "?");
            output = output.Replace("%40", "@");
            output = output.Replace("%5B", "[");
            output = output.Replace("%5C", "\\");
            output = output.Replace("%5D", "]");
            output = output.Replace("%5E", "^");
            output = output.Replace("%5F", "_");

            output = output.Replace("%3a", ":");
            output = output.Replace("%3b", ";");
            output = output.Replace("%3c", "<");
            output = output.Replace("%3d", "=");
            output = output.Replace("%3e", ">");
            output = output.Replace("%3f", "?");
            output = output.Replace("%40", "@");
            output = output.Replace("%5b", "[");
            output = output.Replace("%5c", "\\");
            output = output.Replace("%5d", "]");
            output = output.Replace("%5e", "^");
            output = output.Replace("%5f", "_");

            output = output.Replace("%60", "`");


            return output;
        }

        public string url_encrypt(string page, Dictionary<string, string> inputs)
        {
            string result = "", enc_str = "";
            int count = inputs.Count;
            int i = 0;
            foreach (KeyValuePair<string, string> pair in inputs)
            {
                i++;
                enc_str = enc_str + pair.Key + "=" + pair.Value.ToString();
                if (i != count)
                {
                    enc_str = enc_str + "&";
                }
            }
            result = page + "?" + this.Encrypt(enc_str);
            return result;
        }

        
    }
}
