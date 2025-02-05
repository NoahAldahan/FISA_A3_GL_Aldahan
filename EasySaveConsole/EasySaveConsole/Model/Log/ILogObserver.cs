using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model.Log
{
    internal interface ILogObserver
    {
        void Notify(Dictionary<string, object> message);

    }
}
