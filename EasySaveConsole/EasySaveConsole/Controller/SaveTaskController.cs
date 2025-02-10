﻿using EasySaveConsole.Model;
using EasySaveConsole.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        internal void CreateSaveTask() 
        {
            string saveTaskName = ShowQuestion(EMessage.AskSaveTaskNameMessage);
            string saveTaskSource = ShowQuestion(EMessage.AskSaveTaskSourceFolderMessage);
            string saveTaskTarget = ShowQuestion(EMessage.AskSaveTaskTargetFolderMessage);
            int saveTaskType = int.Parse(ShowQuestion(EMessage.AskSaveTaskType));

            saveTaskManager.AddSaveTask((ESaveTaskTypes)saveTaskType, saveTaskSource, saveTaskTarget, saveTaskName);

            saveTaskManager.ExecuteAllSaveTasks();


        }
    }
}
