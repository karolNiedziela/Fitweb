using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Services
{
    public interface IRngService
    {
        string Generate(int length = 30);
    }
}
