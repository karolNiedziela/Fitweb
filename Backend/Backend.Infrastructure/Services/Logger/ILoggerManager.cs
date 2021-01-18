using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services.Logger
{
    public interface ILoggerManager
    {
        void LogFatal(string message);

        void LogError(string message);

        void LogWarn(string message);

        void LogInfo(string message);

        void LogDebug(string message);

        void LogTrace(string message);
    }
}
