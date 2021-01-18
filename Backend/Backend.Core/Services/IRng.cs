using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Services
{
    public interface IRng
    {
        string Generate(int length = 30);
    }
}
