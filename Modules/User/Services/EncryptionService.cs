using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myapp.Modules.User.Interface;
using System.Security.Cryptography;
using System.IO;


namespace myapp.Modules.User.Services
{
    public class EncryptionService : IEncryptionService
    {
        private readonly byte[] _key; 
        private readonly byte[] _iv;  
        private readonly IConfiguration _config;

        public EncryptionService(IConfiguration config)
        {
            _config = config;
            _key = Encoding.UTF8.GetBytes(_config["Encryted:key"]);
            _iv = Encoding.UTF8.GetBytes(_config["Encryted:iv"]);
        }
        public string DecryptData(string encryptedData)
        {
            if (string.IsNullOrEmpty(encryptedData))
            throw new ArgumentNullException(nameof(encryptedData), "Encrypted data cannot be null or empty.");

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                byte[] buffer = Convert.FromBase64String(encryptedData);

                using (MemoryStream ms = new MemoryStream(buffer))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        public string EncryptData(string data)
        {
            if (string.IsNullOrEmpty(data))
            throw new ArgumentNullException(nameof(data), "Data cannot be null or empty.");

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = _key;
                aesAlg.IV = _iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            // Write the data to be encrypted
                            sw.Write(data);
                        }
                    }

                    // Return the encrypted data as a Base64 string
                    return Convert.ToBase64String(ms.ToArray());
                    }
            }
        }
    }
}