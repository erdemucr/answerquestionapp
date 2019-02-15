using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AqApplication.Manage.Utilities
{
    public static class Formats
    {
        public static string RegularDatetime(this DateTime date)
        {
            if (date == default(DateTime))
                return "-";
            return date.ToString("dd/MM/yyyy");
        }
    }
}
