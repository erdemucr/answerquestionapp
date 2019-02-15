using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AqApplication.Core.Type
{
    public class Result<T> : Result
    {
        public Result()
        {

        }
        public Result(Exception ex):base(ex)
        {
        }
        public T Data { get; set; }
    }
}
