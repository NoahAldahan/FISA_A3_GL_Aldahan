using EasySaveConsole.Model;
using EasySaveConsole.View;
using System.Collections.Generic;
using EasySaveConsole.Utilities;
using System;
using System.Text.RegularExpressions;
using System.Diagnostics.Eventing.Reader;
using System.Collections;
using System.ComponentModel.Design;

namespace EasySaveConsole.Controller
{
    enum ECliSaveTaskAction
    {
        InitMenu = 0,
        ShowSaveTasks = 1,
        StartSaveTasks = 2,
        CreateSaveTask = 3,
        ModifySaveTasks = 4,
        DeleteSaveTasks = 5,
        Quit = 6,
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
            dictActions.Add((int)ECliSaveTaskAction.ShowSaveTasks, () => SaveTaskDetails());
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
        internal void StartSaveTasks()
        {
            ShowAllSaveTask();
            string userInput = ShowQuestion(EMessage.AskSaveTaskIdMessage);
            ProcessSaveTaskSelection(userInput, ECliSaveTaskAction.StartSaveTasks);
        }

        internal void ModifySaveTasks()   
        {
            ShowAllSaveTask();
            string userInput = ShowQuestion(EMessage.AskSaveTaskIdMessage);
            ProcessSaveTaskSelection(userInput, ECliSaveTaskAction.ModifySaveTasks);
        }

        internal void DeleteSaveTask()
        {
            ShowAllSaveTask();
            string userInput = ShowQuestion(EMessage.AskSaveTaskIdMessage);
            ProcessSaveTaskSelection(userInput, ECliSaveTaskAction.DeleteSaveTasks);
        }

        internal void SaveTaskDetails()
        {
            ShowAllSaveTask();
            string userInput = ShowQuestion(EMessage.AskSaveTaskIdMessage);
            ProcessSaveTaskSelection(userInput, ECliSaveTaskAction.ShowSaveTasks);
        }



        internal void ShowAllSaveTaskDetails(int saveTaskId)
        {
            ShowMessage(messagesManager.GetMessageTranslate(EMessage.ShowSaveTaskDetailsMessage));
            ShowMessage(messagesManager.GetMessageTranslate(EMessage.ShowSaveTaskNameMessage)
                + saveTaskManager.GetSaveTaskName(saveTaskId));
            ShowMessage(messagesManager.GetMessageTranslate(EMessage.ShowSaveTaskSourcePathMessage)
                + saveTaskManager.GetSaveTaskSourcePath(saveTaskId));

            ShowMessage(messagesManager.GetMessageTranslate(EMessage.ShowSaveTaskTargetPathMessage)
                + saveTaskManager.GetSaveTaskTargetPath(saveTaskId));

            ShowMessage(messagesManager.GetMessageTranslate(EMessage.ShowSaveTaskTypeMessage)
                + saveTaskManager.GetSaveTaskType(saveTaskId));
        }


        internal EMessage ModifySaveTask(int index)
        {
            ShowMessage(EMessage.ShowLetEmptyRowForDefault);
            //SaveTask name modification
            string saveTaskName = ShowQuestion(messagesManager.GetMessageTranslate(EMessage.AskSaveTaskModifyNameMessage) 
                + $"({saveTaskManager.GetSaveTaskName(index)}) : ");
            if (saveTaskName == "")
            {
                saveTaskName = saveTaskManager.GetSaveTaskName(index);
            }
            //SaveTask path source modification
            string saveTaskSource = ShowQuestion(messagesManager.GetMessageTranslate(EMessage.AskSaveTaskModifySourceFolderMessage) 
                + $" ({saveTaskManager.GetSaveTaskSourcePath(index)}) : ");
            if (saveTaskSource == "") 
            {
                saveTaskSource = saveTaskManager.GetSaveTaskSourcePath(index);
            }
            if (!Utilities.Utilities.IsValidPath(saveTaskSource))
            {
                return EMessage.ErrorSaveTaskPathMessage;
            }
            //SaveTask path target modification
            string saveTaskTarget = ShowQuestion(messagesManager.GetMessageTranslate(EMessage.AskSaveTaskModifyTargetFolderMessage) 
                + $"({saveTaskManager.GetSaveTaskTargetPath(index)}) : ");
            if (saveTaskTarget == "")
            {
                saveTaskTarget = saveTaskManager.GetSaveTaskTargetPath(index);
            }
            if (!Utilities.Utilities.IsValidPath(saveTaskTarget))
            {
                return EMessage.ErrorSaveTaskPathMessage;
            }
            //SaveTaskType modification
            int saveTaskType;
            string saveTaskTypeStr = ShowQuestion(messagesManager.GetMessageTranslate(EMessage.AskSaveTaskModifyType) 
                + $"({saveTaskManager.GetSaveTaskStrType(index)}) : ");
            if(saveTaskTypeStr == "")
            {
                saveTaskType = (int)saveTaskManager.GetSaveTaskType(index);
            }
            else if (!Int32.TryParse(saveTaskTypeStr, out saveTaskType) || !Enum.IsDefined(typeof(ESaveTaskTypes), saveTaskType))
            {
                return EMessage.ErrorSaveTaskTypeMessage;
            }
            saveTaskManager.ModifySaveTask(index, (ESaveTaskTypes)saveTaskType, saveTaskSource, saveTaskTarget, saveTaskName);
            return EMessage.SuccessModifySaveTaskMessage;
        }
        internal void CreateSaveTask()
        {
            ShowAllSaveTask();
            string saveTaskName = ShowQuestion(messagesManager.GetMessageTranslate(EMessage.AskSaveTaskNameMessage));
            if (saveTaskManager.IsSaveTaskNameExist(saveTaskName))
            {
                ShowMessagePause(EMessage.ErrorSaveTaskNameDuplicateMessage);
                return;
            }
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

            saveTaskManager.SerializeSaveTasks();
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
            HandleSaveTasks(indexs, cliSaveTaskAction);
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
            HandleSaveTasks(indexs, cliSaveTaskAction);
        }
        internal void HandleSaveTasks(List<int> indexs, ECliSaveTaskAction cliSaveTaskAction)
        {
            foreach (int index in indexs)
            {
                try
                {
                    switch (cliSaveTaskAction)
                    {
                        case (ECliSaveTaskAction.StartSaveTasks):
                            ShowMessage(messagesManager.GetMessageTranslate(saveTaskManager.ExecuteSaveTask(index)) + saveTaskManager.GetSaveTaskName(index));
                            break;
                        case (ECliSaveTaskAction.DeleteSaveTasks):
                            ShowMessage(saveTaskManager.RemoveSaveTask(index));
                            break;
                        case (ECliSaveTaskAction.ModifySaveTasks):
                            ShowMessage(ModifySaveTask(index));
                            break;
                        case (ECliSaveTaskAction.ShowSaveTasks):
                            ShowAllSaveTaskDetails(index);
                            break;
                        default:
                            break;
                    }
                }
                catch
                {
                    ShowMessagePause(EMessage.ErrorStartSaveTaskMessage);
                }
            }
            ShowQuestion(EMessage.PressKeyToContinue);
        }
    }
}
