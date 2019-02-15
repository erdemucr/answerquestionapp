using AqApplication.Entity.Common;
using System;
using System.Net;

namespace AqApplication.Entity.Logging
{
   public class HostCallLoggingModel: BaseEntity
    {
    
        public string RequestContentType { get; set; }
        public string RequestUri { get; set; }
        public string RequestMethod { get; set; }
        public DateTime? RequestTimestamp { get; set; }
        public string ResponseContentType { get; set; }
        public HttpStatusCode ResponseStatusCode { get; set; }
        public DateTime? ResponseTimestamp { get; set; }

        public long TimeInterval { get; set; }
    }
}
