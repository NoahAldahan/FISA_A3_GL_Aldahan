using EasySaveConsole.Model;
using EasySaveConsole.View;

namespace EasySaveConsole.Controller
{
    enum ECliAction
    {
        InitMenu = 0,
        LanguageMenu = 1,
        SaveMenu = 2,
        Stop = 3
    }
    internal class CliController : BaseController
    {
        private SaveTaskController saveTaskController;
        internal CliController(MessageManager messagesManager, CliView view, SaveTaskController saveTaskController, LanguageController languageController) : base(messagesManager, view)
        {
            this.saveTaskController = saveTaskController;
            stopCondition = (int)ECliAction.Stop;
            initCondition = (int)ECliAction.InitMenu;
            InitDictAction();
        }

        override protected void InitDictAction()
        {
            dictActions.Add((int)ECliAction.InitMenu, () => { ShowMessage(EMessage.MenuMessage); });
            dictActions.Add((int)ECliAction.Stop, () => { ShowMessage(EMessage.StopMessage); });
            dictActions.Add((int)ECliAction.LanguageMenu, () => { ShowQuestion(EMessage.AskLanguageMessage); });
            dictActions.Add((int)ECliAction.SaveMenu, () => { saveTaskController.StartCli(); });
        }
    }
}
