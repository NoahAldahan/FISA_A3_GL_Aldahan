using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model.Log
{
    internal class LogDaily : ILogObserver
    {
        public void Notify(RealTimeInfo RealTime)
        {
            Console.WriteLine("LogDaily do nothing");
        }
        public void Notify(DailyInfo DailyInfo)
        {
          Console.WriteLine(DailyInfo.ToString());
        }

    }
}
