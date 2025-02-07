using EasySaveConsole.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model.Log
{
    internal abstract class LogObserver
    {
        protected readonly JsonLogManager jsonLogManager;

        // Injection du JsonLogManager dans le constructeur
        protected LogObserver(JsonLogManager jsonLogManager)
        {
            this.jsonLogManager = jsonLogManager;
        }

        internal abstract void CreateNotify(RealTimeInfo realTimeInfo);
        internal abstract void UdpateNotify(DailyInfo dailyInfo, RealTimeInfo realTime);
    }

}
