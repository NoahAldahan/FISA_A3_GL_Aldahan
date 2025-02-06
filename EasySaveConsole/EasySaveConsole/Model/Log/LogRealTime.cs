using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model.Log
{
    internal class LogRealTime : ILogObserver
    {
        public void Notify(RealTimeInfo RealTime)
        {
            Console.WriteLine("LogRealTime do nothing");
        }
        public void Notify(DailyInfo DailyInfo)
        {
            Console.WriteLine("LogRealTime do nothing");
        }
    }
}
