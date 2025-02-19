using EasySaveWPFApp.Model;
using System.Collections.Generic;
using EasySaveWPFApp.Utilities;
using System;
using System.Text.RegularExpressions;
using System.Diagnostics.Eventing.Reader;
using System.Collections;
using System.ComponentModel.Design;
using System.Reflection;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml.Linq;

namespace EasySaveWPFApp.ViewModel
{
    // Enum defining the possible CLI save task actions
    public enum ECliSaveTaskAction
    {
        InitMenu = 0,
        ShowSaveTasks = 1,   // Action to initialize the save task menu
        StartSaveTasks = 2,  // Action to display all save tasks
        CreateSaveTask = 3,  // Action to start save tasks
        ModifySaveTasks = 4, // Action to create a new save task
        DeleteSaveTasks = 5, // Action to modify an existing save task
        Quit = 6,            // Action to quit the save task menu
    }
    // Controller class for managing save tasks in the CLI
    public class SaveTaskViewModel : INotifyPropertyChanged
    {
        // Manager for handling save tasks
        public SaveTaskManager saveTaskManager { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;


        // Constructor for the SaveTaskController class
        internal SaveTaskViewModel(SaveTaskManager saveTaskManager)
        {
            this.saveTaskManager = saveTaskManager;
        }
        public ObservableCollection<SaveTask> BindSaveTasks
        {
            get => saveTaskManager.SaveTasks;
            set { saveTaskManager.SaveTasks = value; OnPropertyChanged(nameof(saveTaskManager.SaveTasks)); }
        }
        internal bool ModifySaveTaskSourcePath(string name, string path)
        {
            return saveTaskManager.ModifySaveTaskSourcePath(name, path);
        }

        internal bool ModifySaveTaskTargetPath(string name, string path)
        {
            return saveTaskManager.ModifySaveTaskTargetPath(name, path);
        }

        internal bool ModifySaveTaskName(string currentName, string newName)
        {
           return saveTaskManager.ModifySaveTaskName(currentName, newName);
        }

        internal bool ModifySaveTaskType(string name, ESaveTaskTypes type)
        {
            return saveTaskManager.ModifySaveTaskType(name, type);
        }
        internal void CreateSaveTask(string saveTaskName, string saveTaskSource, string saveTaskTarget, ESaveTaskTypes saveTaskType)
        {
            if (saveTaskManager.IsSaveTaskNameExist(saveTaskName))
            {
                return;
            }
            if (!Utilities.Utilities.IsValidPath(saveTaskSource))
            {
                // Show an error message if the source path is invalid
                return;
            }
            if (!Utilities.Utilities.IsValidPath(saveTaskTarget))
            {
                // Show an error message if the target path is invalid
                return;
            }
            if (!Enum.IsDefined(typeof(ESaveTaskTypes), saveTaskType))
            {
                return;
            }
            saveTaskManager.AddSaveTask((ESaveTaskTypes)saveTaskType, saveTaskSource, saveTaskTarget, saveTaskName);
            saveTaskManager.SerializeSaveTasks();
        }

        internal void HandleSaveTasks(List<int> indexs, ECliSaveTaskAction cliSaveTaskAction)
        {
            indexs.Sort((a, b) => b.CompareTo(a));
            foreach (int index in indexs)
            {
                try
                {
                    switch (cliSaveTaskAction)
                    {
                        case (ECliSaveTaskAction.StartSaveTasks):
                            HandleSaveTaskExecution(index);
                            break;
                        case (ECliSaveTaskAction.DeleteSaveTasks):
                            saveTaskManager.RemoveSaveTask(index);
                            break;
                        default:
                            break;
                    }
                }
                catch
                {
                    return; //error saveTask start or delete
                }
            }
            return;
        }

        internal void HandleSaveTaskExecution(int index)
        {
            bool DidEverythingSaveCorrectly = saveTaskManager.ExecuteSaveTask(index);
            if (DidEverythingSaveCorrectly)
                return; //success saveTaskExecution  //ShowMessage(messagesManager.GetMessageTranslate(EMessage.SuccessStartSaveTaskMessage) + saveTaskManager.GetSaveTaskName(index));
            else
            {
                string str = "ErrorStartSaveTaskMessage"; //ErrorStartSaveTaskMessage
                List<string> UnsavedPaths = saveTaskManager.GetCurrentUnsavedPaths();
                if (UnsavedPaths != null && UnsavedPaths.Count > 0)
                {
                    str += "EMessage.ErrorStartSaveTaskPathListMessage : ";//messagesManager.GetMessageTranslate(EMessage.ErrorStartSaveTaskPathListMessage);
                    foreach (string path in UnsavedPaths)
                    {
                        str += "\n" + path;
                    }
                }
                ////error saveTaskExecution //ShowMessage(str);
            }
        }
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}