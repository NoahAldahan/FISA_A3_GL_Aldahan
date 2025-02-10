using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    public abstract class Log
    {
        public readonly JsonLogManager jsonLogManager;

        // Injection du JsonLogManager dans le constructeur
        public Log(JsonLogManager jsonLogManager)
        {
            this.jsonLogManager = jsonLogManager;
        }
    }

}
