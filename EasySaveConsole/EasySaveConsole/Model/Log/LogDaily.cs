using EasySaveConsole.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model.Log
{
    internal class LogDaily : LogObserver
    {

        internal LogDaily(JsonLogManager jsonLogManager) : base(jsonLogManager){}
        internal override void CreateNotify(RealTimeInfo RealTime)
        {
            jsonLogManager.CreateDailyJsonFile(RealTime.SaveDate);
        }
        internal override void UdpateNotify(DailyInfo DailyInfo, RealTimeInfo RealTimeInfo)
        {
          Console.WriteLine(DailyInfo.ToString());
        }
    }
}
