using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Infrastructure.Services
{
    public interface IEncrypter
    {
        string GetSalt(string value);
        string GetHash(string value, string salt);
    }
}
