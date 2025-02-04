using EasySaveConsole.Model;
using EasySaveConsole.View;

namespace EasySaveConsole.Controller
{
    internal abstract class BaseController
    {
        protected MessageManager messagesManager;
        protected BaseView view;

        protected BaseController(MessageManager messagesManager, BaseView view)
        {
            this.messagesManager = messagesManager;
            this.view = view;
        }

        protected void ShowMessage(EMessage msg)
        {
            string translatedMessage = messagesManager.GetMessageTranslate(msg);
            view.ShowMessage(translatedMessage);
        }

        protected string ShowQuestion(EMessage msg)
        {
            string translatedMessage = messagesManager.GetMessageTranslate(msg);
            return view.ShowQuestion(translatedMessage);
        }
    }
}
