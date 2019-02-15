using AqApplication.Logging.Logger;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace AqApplication.Logging.Providers
{
    public class LoggingDbProvider : ILoggerProvider
    {
        private readonly ConcurrentDictionary<string, HostCallLogger> _loggers = new ConcurrentDictionary<string, HostCallLogger>();
        public LoggingDbProvider()
        {
        }

        public ILogger CreateLogger(string name)
        {
            return new HostCallLogger(name);
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }

}