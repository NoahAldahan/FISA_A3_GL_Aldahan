using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole.Utilities;

namespace EasySaveConsole.Model.Log
{
    internal class LogManager
    {

        internal JsonLogManager jsonLogManager;

        public LogManager(JsonLogManager json) { 
            jsonLogManager = json;
        }


    }
}
