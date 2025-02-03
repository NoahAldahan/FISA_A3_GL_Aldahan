using EasySaveConsole.Model;
using EasySaveConsole.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Controller
{
    internal class LanguageController
    {
        LangagesManager langaguesManager;
        public LanguageController(LangagesManager langaguesManager)
        {
            this.langaguesManager = langaguesManager;
        }
        public Messages SetDefaultLangage(string strLangage)
        {
            Messages msg = langaguesManager.SetDefaultLangage(strLangage);
            return msg;
        }
    }
}
