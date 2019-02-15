using AqApplication.Core.Type;
using AqApplication.Entity.Identity.Data;
using AqApplication.Entity.Logging;
using System;

namespace AqApplication.Repository.Logging
{
    public class LoggingRepo : ILogging, IDisposable
    {
        private readonly int PageSize = 20;
        private ApplicationDbContext context;
        private bool disposedValue = false;
        public LoggingRepo(ApplicationDbContext _context)
        {
            context = _context;
        }

        public Result AddHostCallLogging(HostCallLoggingModel model)
        {
            var result = 0;
            try
            {
                context.HostCallLogging.Add(model);
                result = context.SaveChanges();
            }

            catch (Exception ex)
            {
                return new Result(ex);
            }
            return new Result
            {
                Success = (result > 0)
            };
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    context = null;
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);

        }
    }
}