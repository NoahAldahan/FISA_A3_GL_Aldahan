using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    public abstract class Log
    {
        public string LogRealTimePath;
        public string LogDailyPath;

        public Log(string LogDailyPath, string LogRealTimePath)
        {
            this.LogDailyPath = LogDailyPath;
            this.LogRealTimePath = LogRealTimePath;
        }
    }

}
