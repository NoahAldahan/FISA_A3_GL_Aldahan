using EasySaveWPFApp.Utilities;
using Log;
using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text.Json;

namespace EasySaveWPFApp.Model
{
    public enum ESaveTaskTypes
    {
        Differential = 1, // Represents a differential backup (only modified files).
        Complete = 2, // Represents a complete backup (copies all files).
        Unknown = 3
    }
    internal static class ESaveTaskTypesExtension
    {
        private static readonly Dictionary<ESaveTaskTypes, string> SaveTaskTypesStrings = new Dictionary<ESaveTaskTypes, string>
            {
                { ESaveTaskTypes.Differential, "Differential" },
                { ESaveTaskTypes.Complete, "Complete" },
                {ESaveTaskTypes.Unknown, "Unknown" }
            };

        // Method to get a list of all string representations of supported languages
        internal static List<string> GetAllStrSaveTasksType()
        {
            List<string> strSaveTaskTypes = new List<string>();
            foreach (var type in SaveTaskTypesStrings)
            {
                strSaveTaskTypes.Add(type.Value);
            }
            return strSaveTaskTypes;
        }
        internal static ESaveTaskTypes ToESaveTaskTypes(string ESaveTaskTypesStr)
        {
            foreach (var type in SaveTaskTypesStrings)
            {
                if(type.Value == ESaveTaskTypesStr)
                {
                    return type.Key;
                }
            }
            return ESaveTaskTypes.Unknown;
        }
    }
    // Manages the collection of save tasks, their execution, and persistence.
    public class SaveTaskManager : INotifyPropertyChanged
    {

        // List of all active save tasks.
        public ObservableCollection<SaveTask> SaveTasks { get; set; }
            
        // Factory instance to create new save tasks.
        internal SaveTaskFactory SaveTaskFactory { get; set; }

        private List<string> CurrentUnsavedPaths;

        // The list of encrypting extensions.
        // If a file has the given extension, it will be encrypted after being saved.
        // All encrypting extensions start with "." (e.g. ".txt", ".json").
        private List<string> EncryptingExtensions;

        private readonly ProcessMonitor processMonitor;
        private bool isSoftwareRunning;

        internal List<string> GetCurrentUnsavedPaths()
        {
            return new List<string>(CurrentUnsavedPaths);
        }

        internal List<string> GetEncryptingExtensions()
        {
            return new List<string>(EncryptingExtensions);
        }

        // Returns a copy of the current list of save tasks to avoid unintended modifications.
        internal List<SaveTask> GetSaveTasksClone()
        {
            return new List<SaveTask>(SaveTasks);
        }

        // Constructor: Initializes the save task manager and loads previously saved tasks from JSON.
        internal SaveTaskManager()
        {
            SaveTaskFactory = new SaveTaskFactory();
            // Load the saved tasks from the previous session.
            SaveTasks = new ObservableCollection<SaveTask>(JsonManager.DeserializeSaveTasks());
            EncryptingExtensions = new List<string>(JsonManager.DeserializeEncryptingExtensions());
            CurrentUnsavedPaths = new List<string>();
            processMonitor = new ProcessMonitor();
            processMonitor.OnSoftwareStatusChanged += OnSoftwareStatusChanged;
            isSoftwareRunning = false;
        }
        private void OnSoftwareStatusChanged(bool isRunning)
        {
            isSoftwareRunning = isRunning;
            Console.WriteLine($"Logiciel métier en cours d'exécution : {isRunning}");
        }

        public bool IsSaveTaskNameExist(string name)
        {
            foreach (var task in SaveTasks)
            {
                if (task.name.Equals(name))
                {
                    return true;
                }
            }
            return false;
        }

        internal SaveTask? GetSaveTaskByName(string Name)
        {
            foreach (var task in SaveTasks) 
            {
                if (task.name.Equals(Name)) 
                {
                    return task;
                }
            }
            return null;
        }
        // Add a new save task of type SaveTaskType with sourcePath and targetPath
        internal bool AddSaveTask(ESaveTaskTypes SaveTaskType, string sourcePath, string targetPath, string saveTaskName)
        {
            SaveTasks.Add(SaveTaskFactory.CreateSave(SaveTaskType, sourcePath, targetPath, saveTaskName));
            return true;//EMessage.SuccessCreateSaveTaskMessage;
        }

        // Remove a save task at index
        internal bool RemoveSaveTask(string name)
        {
            try
            {
                SaveTask? saveTaskCurrent = GetSaveTaskByName(name);
                if(saveTaskCurrent == null)
                {
                    return false;
                }
                int index = SaveTasks.IndexOf(saveTaskCurrent);
                SaveTasks.RemoveAt(index);
                return true;// EMessage.SuccessSuppressSaveTaskMessage;
            }
            catch (Exception ex) 
            {
                return false;// EMessage.ErrorSuppressSaveTaskMessage;
            }
        }

        // Starts the save task at index
        internal bool ExecuteSaveTask(string name)
        {
            try
            {
                SaveTask? saveTaskCurrent = GetSaveTaskByName(name);
                if (saveTaskCurrent == null) 
                {
                    return false;
                }
                if (isSoftwareRunning)
                {
                    Console.WriteLine("Impossible d'exécuter la sauvegarde car le logiciel métier est en cours d'exécution.");
                    return false;
                }

                CurrentUnsavedPaths.Clear();
                if (saveTaskCurrent.Save(this))
                {
                    return true;
                }

                CurrentUnsavedPaths = saveTaskCurrent.GetUnsavedPaths();
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'exécution de la tâche : {ex.Message}");
                return false;
            }
        }

        // Modify the save task type
        internal bool ModifySaveTaskType(string name, ESaveTaskTypes newSaveTaskType)
        {
            try
            {

                SaveTask? oldSaveTask = GetSaveTaskByName(name);
                if (oldSaveTask == null)
                {
                    return false;//error
                }
                SaveTask newSaveTask = SaveTaskFactory.CreateSave(
                    newSaveTaskType,
                    oldSaveTask.CurrentDirectoryPair.SourcePath,
                    oldSaveTask.CurrentDirectoryPair.TargetPath,
                    name
                );
                int index = SaveTasks.IndexOf(oldSaveTask);
                // Replace the old save task with the new one.
                SaveTasks.RemoveAt(index);
                SaveTasks.Insert(index, newSaveTask);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        // Modifies the source path of a save task at the specified index.
        internal bool ModifySaveTaskSourcePath(string name, string newSourcePath)
        {
            try
            {
                SaveTask? newSaveTask = GetSaveTaskByName(name);
                if (newSaveTask == null || !Utilities.Utilities.IsValidPath(newSourcePath))
                {
                    return false;//error
                }
                newSaveTask.CurrentDirectoryPair.SourcePath = newSourcePath;
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }

        // Modifies the target path of a save task at the specified index.
        internal bool ModifySaveTaskTargetPath(string name, string newTargetPath)
        {
            try
            {
                SaveTask? newSaveTask = GetSaveTaskByName(name);
                if (newSaveTask == null || !Utilities.Utilities.IsValidPath(newTargetPath))
                {
                    return false;//error
                }
                newSaveTask.CurrentDirectoryPair.TargetPath = newTargetPath;
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }

        }

        // Modifies the target path of a save task at the specified index.
        internal bool ModifySaveTaskName(string name, string newName)
        {
            try
            {
                SaveTask? newSaveTask = GetSaveTaskByName(name);
                if (newSaveTask == null)
                {
                    return false;//error
                }
                newSaveTask.name = newName;
                return true;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }   

        internal bool SwitchSaveTask(string name)
        {
            SaveTask? currentSaveTask = GetSaveTaskByName(name);
            if(currentSaveTask == null)
            {
                return false;
            }
            if (currentSaveTask.BindSaveTaskType == ESaveTaskTypes.Complete)
            {
                ModifySaveTaskType(name, ESaveTaskTypes.Differential);
                return true;
            }
            else if (currentSaveTask.BindSaveTaskType == ESaveTaskTypes.Differential) 
            {
                ModifySaveTaskType(name, ESaveTaskTypes.Complete);
                return true;
            }
            return false;
        }


        // Saves all save tasks config to a json file for persistence
        public void SerializeSaveTasks()
        {
            JsonManager.SerializeSaveTasks(SaveTasks);
        }

        // Sets the encrypting extensions to the given list.
        internal void SetEncryptingExtensions(List<string> newEncryptingExtensions)
        {
            EncryptingExtensions.Clear();
            EncryptingExtensions = new List<string>(newEncryptingExtensions);
        }

        // Serializes the encrypting extensions to a JSON file for persistence.
        public void SerializeEncryptingExtensions()
        {
            JsonManager.SerializeEncryptingExtensions(EncryptingExtensions);
        }

        // Deserializes the encrypting extensions from a JSON file.
        public void DeserializeEncryptingExtensions()
        {
            EncryptingExtensions = JsonManager.DeserializeEncryptingExtensions();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
