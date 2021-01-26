using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Services.Logger
{
    public class LoggerManager : ILoggerManager
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        public LoggerManager()
        {

        }

        public void LogDebug(string message)
        {
            logger.Debug(message);
        }

        public void LogError(string message)
        {
            logger.Error(message);
        }

        public void LogFatal(string message)
        {
            logger.Fatal(message);
        }

        public void LogInfo(string message)
        {
            logger.Info(message);
        }

        public void LogTrace(string message)
        {
            logger.Trace(message);
        }

        public void LogWarn(string message)
        {
            logger.Warn(message);
        }
    }
}
