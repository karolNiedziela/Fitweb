using Backend.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Factories
{
    public interface IRefreshTokenFactory
    {
        RefreshToken Create(int userId);
    }
}
