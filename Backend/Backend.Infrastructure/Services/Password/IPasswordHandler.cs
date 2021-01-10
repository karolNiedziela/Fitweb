using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services
{
    public interface IPasswordHandler 
    {
        string Hash(string password);

        bool IsValid(string hash, string password);
    }
}
