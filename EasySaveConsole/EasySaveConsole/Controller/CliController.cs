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
        private LanguageController languageController;
        internal CliController(MessageManager messagesManager, CliView view, SaveTaskController saveTaskController, LanguageController languageController) : base(messagesManager, view)
        {
            this.saveTaskController = saveTaskController;
            this.languageController = languageController;
            stopCondition = (int)ECliAction.Stop;
            initCondition = (int)ECliAction.InitMenu;
            InitDictAction();
        }

        override protected void InitDictAction()
        {
            dictActions.Add((int)ECliAction.InitMenu, () => { ShowMessage(EMessage.MenuMessage); });
            dictActions.Add((int)ECliAction.Stop, () => { ExitCli(); });
            dictActions.Add((int)ECliAction.LanguageMenu, () => { languageController.StartCli(); });
            dictActions.Add((int)ECliAction.SaveMenu, () => { saveTaskController.StartCli(); });
        }

        private void ExitCli()
        {
            ShowMessage(EMessage.StopMessage);
            saveTaskController.saveTaskManager.SerializeSaveTasks();
        }
    }
}
