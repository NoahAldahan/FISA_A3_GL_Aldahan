using EasySaveConsole.Utilities;
using Log;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text.Json;

namespace EasySaveConsole.Model
{
	internal class SaveTaskManager
	{
        JsonManager jsonManager;
        // All the current save tasks
        internal List<SaveTask> SaveTasks { get; set; }
        // The save task factory to create new save tasks
        internal SaveTaskFactory SaveTaskFactory { get; set; }
        // The maximum number of save tasks that can be created at once
        private static int MaxSaveTasks = 5;

        // Getter for the save tasks
        internal List<SaveTask> GetSaveTasksClone()
        {
            return new List<SaveTask>(SaveTasks);
        }

        // Constructor
        internal SaveTaskManager(JsonManager jsonManager)
        {
            SaveTaskFactory = new SaveTaskFactory();
            this.jsonManager = jsonManager;
            // Load the save tasks that are saved from previous session
            SaveTasks = new List<SaveTask>(this.jsonManager.DeserializeSaveTasks());
        }

        // Add a new save task of type SaveTaskType with sourcePath and targetPath
        internal void AddSaveTask(ESaveTaskTypes SaveTaskType, string sourcePath, string targetPath)
        {
            if (SaveTasks.Count >= MaxSaveTasks)
            {
                throw new Exception("Maximum number of save tasks reached");
            }
            SaveTasks.Add(SaveTaskFactory.CreateSave(SaveTaskType, sourcePath, targetPath));
        }

        // Remove a save task at index
        internal void RemoveSaveTask(int index)
        {
            SaveTasks.RemoveAt(index);
        }

        // Remove a save task of type SaveTaskType with sourcePath and targetPath (stops after the first deletion)
        internal void RemoveSaveTask(ESaveTaskTypes SaveTaskType, string sourcePath, string targetPath)
        {
            if (SaveTasks.Count() == 0)
                return;

            SaveTask MatchingSaveTask = SaveTasks.FirstOrDefault(saveTask => (saveTask.CurrentDirectoryPair.SourcePath == sourcePath && saveTask.CurrentDirectoryPair.TargetPath == targetPath));
            if (MatchingSaveTask != null)
                SaveTasks.Remove(MatchingSaveTask);
        }

        // Starts the save task at index
        internal void ExecuteSaveTask(int index)
        {
            SaveTasks[index].Save();
        }

        // Starts all save tasks
        internal void ExecuteAllSaveTasks()
        {
            foreach (SaveTask saveTask in SaveTasks)
            {
                saveTask.Save();
            }
        }

        // Modify the save task type
        internal void ModifySaveTaskType(int index, ESaveTaskTypes newSaveTaskType)
        {
            SaveTask newSaveTask = SaveTaskFactory.CreateSave(newSaveTaskType, SaveTasks[index].CurrentDirectoryPair.SourcePath, SaveTasks[index].CurrentDirectoryPair.TargetPath);
            SaveTasks.RemoveAt(index);
            SaveTasks.Insert(index, newSaveTask);
        }

        // Modify the save task source path
        internal void ModifySaveTaskSourcePath(int index, string newSourcePath)
        {
            SaveTasks[index].CurrentDirectoryPair.SourcePath = newSourcePath;
        }

        // Modify the save task target path
        internal void ModifySaveTaskTargetPath(int index, string newTargetPath)
        {
            SaveTasks[index].CurrentDirectoryPair.TargetPath = newTargetPath;
        }


        // Saves all save tasks config to a json file for persistence
        public void SerializeSaveTasks()
        {
            jsonManager.SerializeSaveTasks(SaveTasks);
        }
    }
}