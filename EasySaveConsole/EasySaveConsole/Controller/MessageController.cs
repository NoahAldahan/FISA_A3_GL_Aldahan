using EasySaveConsole.Model;
using EasySaveConsole.Utilities;
using System.Collections.Generic;

namespace EasySaveConsole.Controller
{
    internal class MessageController
    {
        MessagesManager messagesManager;
        public MessageController() 
        {
            messagesManager = new MessagesManager();
        }
        public string GetMessage(Messages msg)
        {
            string strMsg = messagesManager.GetMessageTranslate(msg);
            return strMsg;
        }
        public string ChangeDefaultLangage(string strLangage)
        {
            Messages msg = messagesManager.SetDefaultLangage(strLangage);   
            return messagesManager.GetMessageTranslate(msg);
        }
    }
}
