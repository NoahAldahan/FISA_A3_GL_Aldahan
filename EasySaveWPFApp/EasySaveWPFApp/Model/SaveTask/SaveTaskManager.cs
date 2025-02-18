﻿using EasySaveWPFApp.Controller;
using EasySaveWPFApp.Utilities;
using Log;
using Microsoft.SqlServer.Server;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text.Json;

namespace EasySaveWPFApp.Model
{
    // Manages the collection of save tasks, their execution, and persistence.
    internal class SaveTaskManager
    {
        // List of all active save tasks.
        internal List<SaveTask> SaveTasks { get; set; }

        // Factory instance to create new save tasks.
        internal SaveTaskFactory SaveTaskFactory { get; set; }

        // Maximum number of save tasks that can be created simultaneously.
        private static int MaxSaveTasks = 5;

        private List<string> CurrentUnsavedPaths;

        // The list of encrypting extensions.
        // If a file has the given extension, it will be encrypted after being saved.
        // All encrypting extensions start with "." (e.g. ".txt", ".json").
        private List<string> EncryptingExtensions;

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
            SaveTasks = new List<SaveTask>(JsonManager.DeserializeSaveTasks());
            CurrentUnsavedPaths = new List<string>();
        }

        internal bool IsSaveTaskNameExist(string name)
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
        internal string GetSaveTaskName(int index)
        {
            return SaveTasks[index].name;
        }

        internal string GetSaveTaskSourcePath(int index)
        {
            return SaveTasks[index].CurrentDirectoryPair.SourcePath;
        }

        internal string GetSaveTaskTargetPath(int index)
        {
            return SaveTasks[index].CurrentDirectoryPair.TargetPath;
        }

        internal EMessage GetSaveTaskTypeMessage(int index)
        {
            return SaveTasks[index].GetMessageSaveTaskType();
        }

        internal ESaveTaskTypes GetSaveTaskType(int index)
        {
            return SaveTasks[index].GetSaveTaskType();
        }
        // Add a new save task of type SaveTaskType with sourcePath and targetPath
        internal EMessage AddSaveTask(ESaveTaskTypes SaveTaskType, string sourcePath, string targetPath, string saveTaskName)
        {
            if (SaveTasks.Count >= MaxSaveTasks)
            {
                return EMessage.ErrorMaxSaveTaskReachMessage;
            }
            SaveTasks.Add(SaveTaskFactory.CreateSave(SaveTaskType, sourcePath, targetPath, saveTaskName));
            return EMessage.SuccessCreateSaveTaskMessage;
        }

        // Remove a save task at index
        internal EMessage RemoveSaveTask(int index)
        {
            try
            {
                SaveTasks.RemoveAt(index);
                return EMessage.SuccessSuppressSaveTaskMessage;
            }
            catch (Exception ex) 
            {
                return EMessage.ErrorSuppressSaveTaskMessage;
            }
        }

        // Removes a save task that matches the given source and target paths (removes the first match).
        internal void RemoveSaveTask(ESaveTaskTypes SaveTaskType, string sourcePath, string targetPath)
        {
            if (SaveTasks.Count == 0)
                return;

            // Find the first save task matching the given source and target paths.
            SaveTask MatchingSaveTask = SaveTasks.FirstOrDefault(saveTask =>
                saveTask.CurrentDirectoryPair.SourcePath == sourcePath &&
                saveTask.CurrentDirectoryPair.TargetPath == targetPath);

            if (MatchingSaveTask != null)
                SaveTasks.Remove(MatchingSaveTask);
        }

        // Starts the save task at index
        internal bool ExecuteSaveTask(int index)
        {
            try
            {
                CurrentUnsavedPaths.Clear();
                if (SaveTasks[index].Save(EncryptingExtensions))
                {
                    return true;
                }
                CurrentUnsavedPaths = SaveTasks[index].GetUnsavedPaths();
                return false;
            }
            catch (Exception ex) 
            {
                return false;
            }
        }

        // Returns all save tasks.
        internal List<SaveTask> GetAllSaveTask()
        {
            return SaveTasks;
        }

        internal bool IsValidSaveTaskId(int id)
        {
            return id >= 0 && id < SaveTasks.Count;
        }

        // Modify the save task type
        internal void ModifySaveTaskType(int index, ESaveTaskTypes newSaveTaskType, string saveTaskName)
        {
            SaveTask newSaveTask = SaveTaskFactory.CreateSave(
                newSaveTaskType,
                SaveTasks[index].CurrentDirectoryPair.SourcePath,
                SaveTasks[index].CurrentDirectoryPair.TargetPath,
                saveTaskName
            );

            // Replace the old save task with the new one.
            SaveTasks.RemoveAt(index);
            SaveTasks.Insert(index, newSaveTask);
        }

        // Modifies the source path of a save task at the specified index.
        internal void ModifySaveTaskSourcePath(int index, string newSourcePath)
        {
            SaveTasks[index].CurrentDirectoryPair.SourcePath = newSourcePath;
        }

        // Modifies the target path of a save task at the specified index.
        internal void ModifySaveTaskTargetPath(int index, string newTargetPath)
        {
            SaveTasks[index].CurrentDirectoryPair.TargetPath = newTargetPath;
        }

        internal void ModifySaveTask(int index, ESaveTaskTypes saveTaskType, string saveTaskSourcePath, string  saveTaskTargetPath, string saveTaskName)
        {
            if (saveTaskType != SaveTasks[index].GetSaveTaskType())
            {
                // Supprimer l'ancienne tâche
                SaveTasks.RemoveAt(index);
                // Créer une nouvelle tâche avec les nouveaux paramètres
                SaveTask newSaveTask = SaveTaskFactory.CreateSave(saveTaskType, saveTaskSourcePath, saveTaskTargetPath, saveTaskName);
                // Ajouter la nouvelle tâche à la liste
                SaveTasks.Insert(index, newSaveTask);
            }
            else
            {
                SaveTasks[index].CurrentDirectoryPair.SourcePath = saveTaskSourcePath;
                SaveTasks[index].CurrentDirectoryPair.TargetPath = saveTaskTargetPath;
                SaveTasks[index].name = saveTaskName;
            }
        }


        // Saves all save tasks config to a json file for persistence
        public void SerializeSaveTasks()
        {
            JsonManager.SerializeSaveTasks(SaveTasks);
        }

        // Adds an extension to the list.
        public void AddEncryptingExtension(string extension)
        {
            EncryptingExtensions.Add(extension);
        }

        // Removes an extension from the list.
        public void RemoveEncryptingExtension(string extension)
        {
            EncryptingExtensions.Remove(extension);
        }

    }
}
