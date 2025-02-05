using EasySaveConsole.Model;
using EasySaveConsole.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Controller
{
    internal class LanguageController //: BaseController
    {
        LanguageManager langaguesManager;
        internal LanguageController(LanguageManager langaguesManager)//: base(messagesManager, view) 
        {
            this.langaguesManager = langaguesManager;
        }
        internal EMessage SetDefaultLanguage(string strLanguage)
        {
            EMessage msg = langaguesManager.SetDefaultLanguage(strLanguage);
            return msg;
        }
    }
}
