using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    public abstract class Log
    {
        protected string LogRealTimePath;
        protected string LogDailyPath;

        public Log(string LogDailyPath, string LogRealTimePath)
        {
            this.LogDailyPath = LogDailyPath;
            this.LogRealTimePath = LogRealTimePath;
        }
    }

}
