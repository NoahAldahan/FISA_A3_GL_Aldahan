using EasySaveConsole.Model;
using EasySaveConsole.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Controller
{
    internal class SaveTaskController : BaseController
    {
        ECliSaveTaskAction action;
        SaveTaskManager saveTaskManager;
        internal SaveTaskController(MessageManager messagesManager, SaveTaskView view, SaveTaskManager saveTaskManager) : base(messagesManager, view) 
        {
            this.saveTaskManager = saveTaskManager;
        }

        internal void HandleUserInput()
        {
            int userInput = this.view.GetOptionUserInput();
            if (Enum.IsDefined(typeof(ECliSaveTaskAction), userInput))
            {
                switch ((ECliSaveTaskAction)userInput)
                {
                    case ECliSaveTaskAction.Quit:
                        ShowMessage(EMessage.StopMessage);
                        action = ECliSaveTaskAction.Quit;
                        break;
                    case ECliSaveTaskAction.Init:
                        ShowMessage(EMessage.SaveTaskMenuMessage);
                        break;
                    case ECliSaveTaskAction.StartTasks:
                        ShowMessage(EMessage.StartSaveTaskMessage);
                        break;
                    case ECliSaveTaskAction.CreateTask:
                        ShowMessage(EMessage.CreateSaveTaskMessage);
                        break;
                    case ECliSaveTaskAction.ModifyTask:
                        //ShowMessage(Messages.DefaultLanguageChangedSuccessMessage);
                        break;
                    case ECliSaveTaskAction.DeleteTask:
                        //ShowMessage(Messages.DefaultLanguageChangedErrorMessage);
                        break;
                    case ECliSaveTaskAction.Help:
                        //ShowOptions();
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
            action = ECliSaveTaskAction.Init;
            ShowMessage(EMessage.SaveTaskMenuMessage);
            while (action != ECliSaveTaskAction.Quit)
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
