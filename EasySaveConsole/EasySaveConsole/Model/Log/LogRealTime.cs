using EasySaveConsole.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model.Log
{
    internal class LogRealTime : LogObserver
    {
        internal LogRealTime(JsonLogManager jsonLogManager) : base(jsonLogManager) { }
        //Create new SaveTask in json file
        internal override void CreateNotify(RealTimeInfo RealTime)
        {
            Console.WriteLine(RealTime.ToString());
            jsonLogManager.AddSaveToRealTimeFile(RealTime);
        }
        internal override void UdpateNotify(DailyInfo DailyInfo, RealTimeInfo RealTimeInfo)
        {
            Console.WriteLine("LogRealTime do nothing");
        }
    }
}
