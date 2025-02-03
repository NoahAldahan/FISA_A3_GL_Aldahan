using EasySaveConsole.CLI;
using EasySaveConsole.Model;
using EasySaveConsole.View;
using System;

namespace EasySaveConsole.Controller
{
    internal class CliController : BaseController
    {
        CliAction state;
        SaveTaskController saveTaskController;
        public CliController(MessagesManager messagesManager, CliView view, SaveTaskController saveTaskController, LanguageController languageController) : base(messagesManager, view)
        {
            this.messagesManager = messagesManager;
            this.view = view;
            this.saveTaskController = saveTaskController;
            state = new CliAction();
        }

        internal void HandleUserInput()
        {
            int userInput = view.GetOptionUserInput();
            if (Enum.IsDefined(typeof(CliAction), userInput))
            {
                switch (userInput)
                {
                    case ((int)CliAction.Stop):
                        ShowMessage(Messages.StopMessage);
                        break;
                    case ((int)CliAction.Init):
                        ShowMessage(Messages.InitMessage);
                        break;
                    case ((int)CliAction.Langages):
                        ShowMessage(Messages.LangagesMessage);
                        break;
                    case ((int)CliAction.ChangeDefaultLangage):
                        string langageChoice = ShowQuestion(Messages.AskLangageMessage);
                        //Messages result = SetDefaultLangage(langageChoice);
                        //ShowMessage(result);
                        break;
                    case ((int)CliAction.SaveMenu):
                        //showMenu
                        saveTaskController.startCli();
                        break;
                }
            }
            else
            {
                ShowMessage(Messages.ErrorUserEntryOptionMessage);
            }
        }

        internal void startCli()
        {
            state = CliAction.Init;
            ShowMessage(Messages.InitMessage);
            while (state != CliAction.Stop)
            {
                try
                {
                    HandleUserInput();
                }
                catch (Exception ex)
                {
                    ShowMessage(Messages.ErrorUserEntryStrMessage);
                }
            }
        }
    }
}
