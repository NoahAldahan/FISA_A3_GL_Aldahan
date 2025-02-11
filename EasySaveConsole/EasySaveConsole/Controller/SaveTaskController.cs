using EasySaveConsole.Model;
using EasySaveConsole.View;
using System.Collections.Generic;
using EasySaveConsole.Utilities;
using System;
using System.Text.RegularExpressions;

namespace EasySaveConsole.Controller
{
    enum ECliSaveTaskAction
    {
        InitMenu = 0,
        ShowTasks = 1,
        StartTasks = 2,
        CreateTask = 3,
        ModifyTask = 4,
        DeleteTask = 5,
        Help = 6,
        Quit = 7
    }
    internal class SaveTaskController : BaseController
    {
        internal SaveTaskManager saveTaskManager;
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
            dictActions.Add((int)ECliSaveTaskAction.ShowTasks, () => WrapperShowAllSaveTask());
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
            ShowAllSaveTask();
            string saveTaskSave = ShowQuestion(EMessage.AskSaveTaskNameMessage);
            string patternRange = @"^\d+-\d+$";
            string patternList = @"^\d+(;\d+)*$";
            string patternSingle = @"^\d+$";
            if (Regex.IsMatch(saveTaskSave, patternRange))
                HandleSaveTaskRange(saveTaskSave);
            else if (Regex.IsMatch(saveTaskSave, patternList))
                HandleSaveTaskList(saveTaskSave);
            else if (Regex.IsMatch(saveTaskSave, patternSingle)) // Valeur unique ex: 1
                HandleSaveTask(saveTaskSave);
            else
                ShowQuestion(EMessage.PressKeyToContinue);
        }

        internal void HandleSaveTaskRange(string SaveTaskSave)
        {
            string[] rangeParts = SaveTaskSave.Split('-');
            int start = int.Parse(rangeParts[0]);
            int end = int.Parse(rangeParts[1]);
            saveTaskManager.ExecuteSaveTaskRange(start, end);

        }

        internal void HandleSaveTaskList(string SavetsaveTaskSave)
        {
            string[] valuesStr = SavetsaveTaskSave.Split(';');
            List<int> indexs = new List<int>();
            try
            {
                foreach (string val in valuesStr)
                {
                    indexs.Add(int.Parse(val));
                }
            }
            catch
            {
                Console.WriteLine("Votre entrée n'est pas du int");
            }
            Console.WriteLine($"Liste de sauvegardes détectée: {string.Join(", ", valuesStr)}");
            saveTaskManager.ExecuteSaveTaskList(indexs);
            ShowQuestion(EMessage.PressKeyToContinue);
        }

        internal void HandleSaveTask(string SaveTaskSave)
        {
            Console.WriteLine($"Sauvegarde unique détectée: {SaveTaskSave}");
            saveTaskManager.ExecuteSaveTask(int.Parse(SaveTaskSave));
            ShowQuestion(EMessage.PressKeyToContinue);
        }
        internal void ShowAllSaveTask()
        {
            List<SaveTask> saveTasks = saveTaskManager.GetAllSaveTask();
            ShowMessage(EMessage.ShowSaveTaskRegisterMessage);
            int id = 0;
            foreach(SaveTask saveTask in saveTasks)
            {
                ShowMessage($" {id} : {saveTask.name}");
                id++;
            }
        }
        internal void WrapperShowAllSaveTask()
        {
            ShowAllSaveTask();
            ShowQuestion(EMessage.PressKeyToContinue);
        }
    }
}
