using Microsoft.Extensions.Logging;
using System;


namespace AqApplication.Logging.Logger
{
    public class HostCallLogger : ILogger
    {
        private readonly string _name;

        public HostCallLogger(string name)
        {
            _name = name;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {

        }
    }
}
