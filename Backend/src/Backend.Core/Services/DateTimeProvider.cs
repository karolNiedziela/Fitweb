using Backend.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Backend.Core.Services
{

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
