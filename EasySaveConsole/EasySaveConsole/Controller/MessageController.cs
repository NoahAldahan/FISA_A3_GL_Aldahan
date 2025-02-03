using EasySaveConsole.Model;
using EasySaveConsole.Utilities;
using System;
using System.Collections.Generic;

namespace EasySaveConsole.Controller
{
    internal class MessageController
    {
        MessagesManager messagesManager;
        public MessageController(MessagesManager messagesManagerArg) 
        {
            messagesManager = messagesManagerArg;
        }
        public string GetMessage(Messages msg)
        {
            string strMsg = messagesManager.GetMessageTranslate(msg);
            return strMsg;
        }
        public Messages SetDefaultLangage(string strLangage)
        {
            Messages msg = messagesManager.SetDefaultLangage(strLangage);   
            return msg;
        }
    }
}
