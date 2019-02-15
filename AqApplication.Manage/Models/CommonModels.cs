using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AqApplication.Manage.Models
{
    public class CommonBreadcrumbModel
    {
       public string Pagename { get; set; }
        public string Backurl { get; set; }
        public string Backpagename { get; set; }

        public string Message { get; set; }

        public bool? isSuccess { get; set; }
    }
}