using EasySaveConsole.Model;
using EasySaveConsole.View;
using System.Collections.Generic;
using EasySaveConsole.Utilities;
using System;
using System.Text.RegularExpressions;
using System.Diagnostics.Eventing.Reader;

namespace EasySaveConsole.Controller
{
    enum ECliSaveTaskAction
    {
        InitMenu = 0,
        ShowTasks = 1,
        StartSaveTasks = 2,
        CreateSaveTask = 3,
        ModifySaveTasks = 4,
        DeleteSaveTasks = 5,
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
            dictActions.Add((int)ECliSaveTaskAction.CreateSaveTask, () => CreateSaveTask());
            dictActions.Add((int)ECliSaveTaskAction.StartSaveTasks, () => StartSaveTasks());
            dictActions.Add((int)ECliSaveTaskAction.DeleteSaveTasks, () => DeleteSaveTask());
            dictActions.Add((int)ECliSaveTaskAction.ModifySaveTasks, () => ModifySaveTasks());
            dictActions.Add((int)ECliSaveTaskAction.ShowTasks, () => WrapperShowAllSaveTask());
        }

        internal void WrapperShowAllSaveTask()
        {
            ShowAllSaveTask();
            ShowQuestion(EMessage.PressKeyToContinue);
        }

        internal void ShowAllSaveTask()
        {
            List<SaveTask> saveTasks = saveTaskManager.GetAllSaveTask();
            ShowMessage(EMessage.ShowSaveTaskRegisterMessage);
            int id = 0;
            foreach (SaveTask saveTask in saveTasks)
            {
                ShowMessage($" {id} : {saveTask.name}");
                id++;
            }
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
            string userInput = ShowQuestion(EMessage.AskSaveTaskNameMessage);
            ProcessSaveTaskSelection(userInput, ECliSaveTaskAction.StartSaveTasks);
        }

        internal void ModifySaveTasks()
        {
            ShowAllSaveTask();
            string userInput = ShowQuestion(EMessage.AskSaveTaskNameMessage);
            ProcessSaveTaskSelection(userInput, ECliSaveTaskAction.ModifySaveTasks);
        }

        internal void DeleteSaveTask()
        {
            ShowAllSaveTask();
            string userInput = ShowQuestion(EMessage.AskSaveTaskNameMessage);
            ProcessSaveTaskSelection(userInput, ECliSaveTaskAction.DeleteSaveTasks);
        }

        internal void ProcessSaveTaskSelection(string userInputSaveTask, ECliSaveTaskAction cliSaveTaskAction)
        {
            string patternRange = @"^\d+-\d+$";
            string patternList = @"^\d+(;\d+)*$";
            string patternSingle = @"^\d+$";
            if (Regex.IsMatch(userInputSaveTask, patternRange))
                ParseSaveTaskRange(userInputSaveTask, cliSaveTaskAction);
            else if (Regex.IsMatch(userInputSaveTask, patternList) || Regex.IsMatch(userInputSaveTask, patternSingle))
                ParseSaveTaskList(userInputSaveTask, cliSaveTaskAction);
            else
                ShowQuestion(EMessage.ErrorUserEntryStrMessage);
        }

        internal void ParseSaveTaskRange(string userInputSaveTask, ECliSaveTaskAction cliSaveTaskAction)
        {
            string[] rangeParts = userInputSaveTask.Split('-');
            int start = int.Parse(rangeParts[0]);
            int end = int.Parse(rangeParts[1]);
            List<int> indexs = new List<int>();
            if(start > end)
            {
                ShowMessagePause(EMessage.ErrorStartEndIndexSaveTaskMessage);
                return;
            }
            else if(saveTaskManager.IsValidSaveTaskId(start) && saveTaskManager.IsValidSaveTaskId(end))
            {
                ShowMessagePause(EMessage.ErrorSaveTaskNotFoundMessage);
                return;
            }
            for (int i = start; i <= end; i++)
            {
                indexs.Add(i);
            }
            HandleSaveTask(indexs, cliSaveTaskAction);
            ShowQuestion(EMessage.PressKeyToContinue);
        }

        internal void ParseSaveTaskList(string SaveTask, ECliSaveTaskAction cliSaveTaskAction)
        {
            string[] valuesStr = SaveTask.Split(';');
            int index;
            List<int> indexs = new List<int>();
            try
            {
                foreach (string val in valuesStr)
                {
                    index = int.Parse(val);
                    if (!saveTaskManager.IsValidSaveTaskId(index))
                    {
                        ShowMessagePause(EMessage.ErrorSaveTaskNotFoundMessage);
                        return;
                    }
                    indexs.Add(int.Parse(val));
                }
            }
            catch
            {
                ShowMessagePause(EMessage.ErrorMessage);
            }
            HandleSaveTask(indexs, cliSaveTaskAction);
        }
        internal void HandleSaveTask(List<int> indexs, ECliSaveTaskAction cliSaveTaskAction)
        {
            switch (cliSaveTaskAction)
            {
                case ECliSaveTaskAction.StartSaveTasks:
                    EMessage msg = saveTaskManager.ExecuteSaveTaskList(indexs);
                    ShowMessagePause(msg);
                    break;
                //case ECliSaveTaskAction.:
                //    saveTaskManager.ExecuteSaveTaskList(indexs);
                //    break;
            }
        }
    }
}
