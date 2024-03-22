using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatNet.Services.EncryptionAppService
{
    public interface IEncryptionAppService
    {
        string EncryptKey(string cadena);
        string DecryptKey(string clave);
    }
}
