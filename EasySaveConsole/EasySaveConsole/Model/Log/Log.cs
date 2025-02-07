using EasySaveConsole.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model.Log
{
    internal abstract class Log
    {
        protected readonly JsonLogManager jsonLogManager;

        // Injection du JsonLogManager dans le constructeur
        protected Log(JsonLogManager jsonLogManager)
        {
            this.jsonLogManager = jsonLogManager;
        }
    }

}
