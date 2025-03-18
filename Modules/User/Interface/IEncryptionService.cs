using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace myapp.Modules.User.Interface
{
    public interface IEncryptionService
    {
        string EncryptData(string data);
        string DecryptData(string encryptedData);
    }
}