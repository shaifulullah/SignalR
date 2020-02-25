namespace MDA.Security
{
    using System;
    using System.IO;
    using System.Security.Cryptography;

    public static class Security
    {
        private static string KEY = "Fga8Z2RyKKozuTs5HZB8Cm15ZR9zErK8zK5p4FcKlIA=";
        private static string VECTOR = "AyT+/suLwalPiIYqb+OGaw==";

        /// <summary>
        /// Decrypt
        /// </summary>
        /// <param name="encrypted">Encrypted String</param>
        /// <returns>Decrypted String</returns>
        public static string Decrypt(string encrypted)
        {
            string decrypted;
            using (var rijndael = Rijndael.Create())
            {
                var decryptor = rijndael.CreateDecryptor(Convert.FromBase64String(KEY), Convert.FromBase64String(VECTOR));
                using (var msDecrypt = new MemoryStream(Convert.FromBase64String(encrypted)))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            decrypted = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            return decrypted;
        }

        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="text">String to Encrypt</param>
        /// <returns>Encrypted String</returns>
        public static string Encrypt(string text)
        {
            byte[] encrypted;
            using (var rijndael = Rijndael.Create())
            {
                var encryptor = rijndael.CreateEncryptor(Convert.FromBase64String(KEY), Convert.FromBase64String(VECTOR));
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(text);
                        }

                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(encrypted, 0, encrypted.Length);
        }
    }
}