using EasySaveConsole.Model;
using System;
using EasySaveConsole.View;

namespace EasySaveConsole.Controller
{
    internal class CliController : BaseController
    {
        ECliAction action;
        SaveTaskController saveTaskController;
        internal CliController(MessageManager messagesManager, CliView view, SaveTaskController saveTaskController, LanguageController languageController) : base(messagesManager, view)
        {
            this.saveTaskController = saveTaskController;
        }

        internal void HandleUserInput()
        {
            int userInput = this.view.GetOptionUserInput();
            if (Enum.IsDefined(typeof(ECliAction), userInput))
            {
                switch ((ECliAction)userInput)
                {
                    case ECliAction.Stop:
                        ShowMessage(EMessage.StopMessage);
                        break;
                    case ECliAction.Init:
                        ShowMessage(EMessage.InitMessage);
                        break;
                    case ECliAction.Languages:
                        ShowMessage(EMessage.LanguagesMessage);
                        break;
                    case ECliAction.ChangeDefaultLanguage:
                        string languageChoice = ShowQuestion(EMessage.AskLanguageMessage);
                        //Messages result = SetDefaultLanguage(languageChoice);
                        //ShowMessage(result);
                        break;
                    case ECliAction.SaveMenu:
                        //showMenu
                        saveTaskController.StartCli();
                        break;
                }
            }
            else
            {
                ShowMessage(EMessage.ErrorUserEntryOptionMessage);
            }
        }

        internal void StartCli()
        {
            action = ECliAction.Init;
            ShowMessage(EMessage.InitMessage);
            while (action != ECliAction.Stop)
            {
                try
                {
                    HandleUserInput();
                }
                catch (Exception ex)
                {
                    ShowMessage(EMessage.ErrorUserEntryStrMessage);
                }
            }
        }
    }
}
