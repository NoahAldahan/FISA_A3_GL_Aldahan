using EasySaveConsole.CLI;
using EasySaveConsole.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole.View;

namespace EasySaveConsole.Controller
{
    internal abstract class BaseController
    {
        protected MessagesManager messagesManager;
        protected BaseView view;

        public BaseController(MessagesManager messagesManager, BaseView view)
        {
            this.messagesManager = messagesManager;
            this.view = view;
        }

        protected void ShowMessage(Messages msg)
        {
            string translatedMessage = messagesManager.GetMessageTranslate(msg);
            view.ShowMessage(translatedMessage);
        }

        protected string ShowQuestion(Messages msg)
        {
            string translatedMessage = messagesManager.GetMessageTranslate(msg);
            return view.ShowQuestion(translatedMessage);
        }
    }
}
