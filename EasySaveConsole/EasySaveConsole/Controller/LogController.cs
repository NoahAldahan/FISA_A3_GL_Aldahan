using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Controller
{
    internal class LogController
    {
        // Enum defining the possible CLI language actions
        enum ECliLogAction
        {
            InitMenu = 0,    // Action to initialize the language menu
            ChangeLog = 1, // Action to change the language
            Quit = 2         // Action to quit the language menu
        }
        public LogController() { }

    }
}
