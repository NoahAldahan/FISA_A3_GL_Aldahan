using EasySaveConsole.Model;
using EasySaveConsole.View;
using EasySaveConsole.Utilities;
using System;
using System.Text.RegularExpressions;

namespace EasySaveConsole.Controller
{
    enum ECliSaveTaskAction
    {
        InitMenu = 0,
        StartTasks = 1,
        CreateTask = 2,
        ModifyTask = 3,
        DeleteTask = 4,
        Help = 5,
        Quit = 6
    }
    internal class SaveTaskController : BaseController
    {
        SaveTaskManager saveTaskManager;
        internal SaveTaskController(MessageManager messagesManager, SaveTaskView view, SaveTaskManager saveTaskManager) : base(messagesManager, view) 
        {
            this.saveTaskManager = saveTaskManager;
            stopCondition = (int)ECliSaveTaskAction.Quit;
            initCondition = (int)ECliAction.InitMenu;
            InitDictAction();
        }
        override protected void InitDictAction()
        {
            dictActions.Add((int)ECliSaveTaskAction.InitMenu, () => { ShowMessage(EMessage.MenuSaveTaskMessage); });
            dictActions.Add((int)ECliSaveTaskAction.Quit, () => { ShowMessage(EMessage.StopMessage); });
            dictActions.Add((int)ECliSaveTaskAction.CreateTask, () => CreateSaveTask());
            dictActions.Add((int)ECliSaveTaskAction.StartTasks, () => StartSaveTasks());
        }

        internal void CreateSaveTask() 
        {
            string saveTaskName = ShowQuestion(EMessage.AskSaveTaskNameMessage);
            string saveTaskSource = ShowQuestion(EMessage.AskSaveTaskSourceFolderMessage);
            if (!Utilities.Utilities.IsValidPath(saveTaskSource))
            {
                ShowMessagePause(EMessage.ErrorSaveTaskPathMessage);
                return;
            }   
            string saveTaskTarget = ShowQuestion(EMessage.AskSaveTaskTargetFolderMessage);
            if (!Utilities.Utilities.IsValidPath(saveTaskTarget))
            {
                ShowMessagePause(EMessage.ErrorSaveTaskPathMessage);
                return;
            }
            int saveTaskType = int.Parse(ShowQuestion(EMessage.AskSaveTaskType));
            if (!Enum.IsDefined(typeof(ESaveTaskTypes), saveTaskType)) 
            {
                ShowMessagePause(EMessage.ErrorSaveTaskTypeMessage);
                return;
            }
            saveTaskManager.AddSaveTask((ESaveTaskTypes)saveTaskType, saveTaskSource, saveTaskTarget, saveTaskName);
            ShowMessagePause(EMessage.SaveTaskAddSuccessMessage);
        }

        internal void StartSaveTasks()
        {
            string saveTaskSave = ShowQuestion(EMessage.AskSaveTaskNameMessage);
            string patternRange = @"^\d+-\d+$";
            string patternList = @"^\d+(;\d+)*$";
            string patternSingle = @"^\d+$";
            if (Regex.IsMatch(saveTaskSave, patternRange))
            {
                string[] rangeParts = saveTaskSave.Split('-');
                int start = int.Parse(rangeParts[0]);
                int end = int.Parse(rangeParts[1]);
                Console.WriteLine($"Plage détectée: {start} à {end}");
                ShowQuestion(EMessage.PressKeyToContinue);
            }
            else if(Regex.IsMatch(saveTaskSave, patternList))
            {
                string[] values = saveTaskSave.Split(';');
                Console.WriteLine($"Liste de sauvegardes détectée: {string.Join(", ", values)}");
                ShowQuestion(EMessage.PressKeyToContinue);
            }
            else if(Regex.IsMatch(saveTaskSave, patternSingle)) // Valeur unique ex: 1
            {
                Console.WriteLine($"Sauvegarde unique détectée: {saveTaskSave}");
                ShowQuestion(EMessage.PressKeyToContinue);
            }
            else
            {
                Console.WriteLine("error");
                ShowQuestion(EMessage.PressKeyToContinue);
            }
        }
    }
}
