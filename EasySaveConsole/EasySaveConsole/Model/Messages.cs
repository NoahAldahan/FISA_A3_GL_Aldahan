using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Utilities
{

    internal class MessagesManager
    {
        LangagesManager langagesManager;
        JsonManager jsonManager;
        public MessagesManager()
        {
            jsonManager = new JsonManager();
            langagesManager = new LangagesManager();
        }
        public string GetMessage(Messages message)
        {

            return jsonManager.GetMessage(message, langagesManager.defaultLangage);
        }
    }
}
