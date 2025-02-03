using EasySaveConsole.CLI;
using EasySaveConsole.Model;
using EasySaveConsole.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Controller
{
    enum ESaveTaskAction
    {
        Init = 0,
        StartTasks = 1,
        CreateTask = 2,
        ModifyTask = 3,
        DeleteTask = 4,
        Help = 5,
        Quit = 6
    }
    internal class SaveTaskController : BaseController
    {
        ESaveTaskAction action;
        SaveTaskManager saveTaskManager;
        public SaveTaskController(MessagesManager messagesManager, SaveTaskView view, SaveTaskManager saveTaskManager) : base(messagesManager, view) 
        {
            this.saveTaskManager = saveTaskManager;
        }

        public void HandleUserInput()
        {
            action = (ESaveTaskAction)this.view.GetOptionUserInput();
            if (Enum.IsDefined(typeof(ESaveTaskAction), action))
            {
                switch (action)
                {
                    case ESaveTaskAction.Quit:
                        ShowMessage(Messages.StopMessage);
                        action = ESaveTaskAction.Quit;
                        break;
                    case ESaveTaskAction.Init:
                        ShowMessage(Messages.SaveTaskMenuMessage);
                        break;
                    case ESaveTaskAction.StartTasks:
                        ShowMessage(Messages.StartSaveTaskMessage);
                        break;
                    case ESaveTaskAction.CreateTask:
                        ShowMessage(Messages.CreateSaveTaskMessage);
                        break;
                    case ESaveTaskAction.ModifyTask:
                        //ShowMessage(Messages.DefaultLanguageChangedSuccessMessage);
                        break;
                    case ESaveTaskAction.DeleteTask:
                        //ShowMessage(Messages.DefaultLanguageChangedErrorMessage);
                        break;
                    case ESaveTaskAction.Help:
                        //ShowOptions();
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
            action = ESaveTaskAction.Init;
            ShowMessage(Messages.SaveTaskMenuMessage);
            while (action != ESaveTaskAction.Quit)
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
