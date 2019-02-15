using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using System.Diagnostics;
using AqApplication.Entity.Logging;
using System;

namespace AqApplication.Service.Utilities
{
    public class LoggingHandler: DelegatingHandler
    {
        private readonly ILogger<LoggingHandler> _logger;

        public LoggingHandler(ILogger<LoggingHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();

            _logger.LogInformation("Starting request");

            var logMetadata = BuildRequestMetadata(request);
            //await SendToLog(logMetadata);

            var response = await base.SendAsync(request, cancellationToken);

            logMetadata.TimeInterval = sw.ElapsedMilliseconds;

            logMetadata = BuildResponseMetadata(logMetadata, response);

            _logger.LogInformation($"Finished request in {sw.ElapsedMilliseconds}ms");

            return response;
        }
        private HostCallLoggingModel BuildRequestMetadata(HttpRequestMessage request)
        {
            HostCallLoggingModel log = new HostCallLoggingModel
            {
                RequestMethod = request.Method.Method,
                RequestTimestamp = DateTime.Now,
                RequestUri = request.RequestUri.ToString()
            };
            return log;
        }
        private HostCallLoggingModel BuildResponseMetadata(HostCallLoggingModel logMetadata, HttpResponseMessage response)
        {
            logMetadata.ResponseStatusCode = response.StatusCode;
            logMetadata.ResponseTimestamp = DateTime.Now;
            logMetadata.ResponseContentType = response.Content.Headers.ContentType.MediaType;
            return logMetadata;
        }
    }

    public interface ILogger
    {
        void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter);
        bool IsEnabled(LogLevel logLevel);
        IDisposable BeginScope<TState>(TState state);
    }

    //public class FileLogProvider : ILoggerProvider
    //{
    //    public ILogger CreateLogger(string category)
    //    {
    //        return new FileLogger();
    //    }
    //    public void Dispose()
    //    {

    //    }
    //}
}
