using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Services
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
