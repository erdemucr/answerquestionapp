using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AqApplication.Core.Thread
{
   public class Tasks
    {
        public Tasks()
        {
            Task thread = new Task(() => SayHello());

         
        }
        public async Task<string> StringResult()
        {
             await Task.Delay(1000);
            return await new Task<string>(() => SayHello());
        }

        public string SayHello()
        {
            return "Naber";
        }
    }
}
