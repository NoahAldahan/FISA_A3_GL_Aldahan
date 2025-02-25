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
using System.Diagnostics;

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
    // ViewModel class for managing save tasks
    public class SaveTaskViewModel : INotifyPropertyChanged
    {
        // Manager for handling save tasks
        public SaveTaskManager saveTaskManager { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public List<String> EcliSaveTaskTypesStr { get; set; }


        // Constructor for the SaveTaskController class
        internal SaveTaskViewModel(SaveTaskManager saveTaskManager)
        {
            this.saveTaskManager = saveTaskManager;
            EcliSaveTaskTypesStr = ESaveTaskTypesExtension.GetAllStrSaveTasksType();
        }
        public ObservableCollection<SaveTask> BindSaveTasks
        {
            get => saveTaskManager.SaveTasks;
            set { saveTaskManager.SaveTasks = value; OnPropertyChanged(nameof(BindSaveTasks)); }
        }
        internal bool ModifySaveTaskSourcePath(string name, string path)
        {
            bool wasSuccessful = saveTaskManager.ModifySaveTaskSourcePath(name, path);
            saveTaskManager.SerializeSaveTasks();
            return wasSuccessful;
        }

        internal bool ModifySaveTaskTargetPath(string name, string path)
        {
            bool wasSuccessful = saveTaskManager.ModifySaveTaskTargetPath(name, path);
            saveTaskManager.SerializeSaveTasks();
            return wasSuccessful;
        }

        internal bool ModifySaveTaskName(string currentName, string newName)
        {
            bool wasSuccessful = saveTaskManager.ModifySaveTaskName(currentName, newName);
            saveTaskManager.SerializeSaveTasks();
            return wasSuccessful;
        }

        internal bool ModifySaveTaskType(string name, ESaveTaskTypes type)
        {
            bool wasSuccessful = saveTaskManager.ModifySaveTaskType(name, type);
            saveTaskManager.SerializeSaveTasks();
            return wasSuccessful;
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


        internal async Task<Dictionary<string, List<string>>> ExecuteSaveTaskAsync(string name)
        {
            bool DidEverythingSaveCorrectly = await saveTaskManager.ExecuteSaveTaskAsync(name);
            Trace.WriteLine("ExecuteSaveTaskAsync STVM before IF");
            if (DidEverythingSaveCorrectly)
            {
                Trace.WriteLine("ExecuteSaveTaskAsync STVM IF");
                return new Dictionary<string, List<string>>(); //success saveTaskExecution  //ShowMessage(messagesManager.GetMessageTranslate(EMessage.SuccessStartSaveTaskMessage) + saveTaskManager.GetSaveTaskName(index));
            }
            else
            {
                Trace.WriteLine("ExecuteSaveTaskAsync STVM ELSE");
                return saveTaskManager.GetCurrentUnsavedPathsDictionary();
            }
        }

        internal void RemoveSaveTask(string name)
        {
            bool DidEverythingSaveCorrectly = saveTaskManager.RemoveSaveTask(name);
            if (DidEverythingSaveCorrectly)
                return; //success saveTaskExecution  //ShowMessage(messagesManager.GetMessageTranslate(EMessage.SuccessStartSaveTaskMessage) + saveTaskManager.GetSaveTaskName(index));
            else
            {

                // TODO : show error in pop up
            }
            saveTaskManager.SerializeSaveTasks();
        }

        internal void SwitchSaveTaskType(string name)
        {
            saveTaskManager.SwitchSaveTask(name);
            saveTaskManager.SerializeSaveTasks();
        }
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void OnWindowClosing(object? sender, CancelEventArgs e)
        {
            saveTaskManager.SerializeSaveTasks();
            saveTaskManager.SerializeEncryptingExtensions();
        }
    }
}