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
    // Manages the collection of save tasks, their execution, and persistence.
    internal class SaveTaskManager
    {
        // List of all active save tasks.
        internal List<SaveTask> SaveTasks { get; set; }

        // Factory instance to create new save tasks.
        internal SaveTaskFactory SaveTaskFactory { get; set; }

        // Maximum number of save tasks that can be created simultaneously.
        private static int MaxSaveTasks = 5;

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
        }

        // Adds a new save task of the specified type, ensuring the maximum limit is not exceeded.
        internal void AddSaveTask(ESaveTaskTypes SaveTaskType, string sourcePath, string targetPath, string saveTaskName)
        {
            if (SaveTasks.Count >= MaxSaveTasks)
            {
                throw new Exception("Maximum number of save tasks reached");
            }
            SaveTasks.Add(SaveTaskFactory.CreateSave(SaveTaskType, sourcePath, targetPath, saveTaskName));
        }

        // Removes a save task at the specified index.
        internal void RemoveSaveTask(int index)
        {
            SaveTasks.RemoveAt(index);
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

        // Executes the save task at the specified index.
        internal void ExecuteSaveTask(int index)
        {
            SaveTasks[index].Save();
        }

        // Executes a range of save tasks from `start` to `stop` indices (inclusive).
        internal void ExecuteSaveTaskRange(int start, int stop)
        {
            if (start < 0 || stop >= SaveTasks.Count || start > stop)
            {
                throw new ArgumentOutOfRangeException(nameof(start), "Invalid start or stop index.");
            }

            for (int i = start; i <= stop; i++)
            {
                SaveTasks[i].Save();
            }
        }

        // Executes a list of specific save tasks based on their indices.
        internal void ExecuteSaveTaskList(List<int> indexs)
        {
            if (indexs == null || indexs.Count == 0)
            {
                throw new ArgumentException("The index list cannot be null or empty.", nameof(indexs));
            }

            foreach (int index in indexs)
            {
                if (index < 0 || index >= SaveTasks.Count)
                {
                    throw new ArgumentOutOfRangeException(nameof(indexs), $"Index {index} is out of range.");
                }

                SaveTasks[index].Save();
            }
        }

        // Executes all save tasks in the list.
        internal void ExecuteAllSaveTasks()
        {
            foreach (SaveTask saveTask in SaveTasks)
            {
                saveTask.Save();
            }
        }

        // Returns all save tasks.
        internal List<SaveTask> GetAllSaveTask()
        {
            return SaveTasks;
        }

        // Modifies the type of an existing save task and replaces it with a new one.
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

        // Saves all save tasks to a JSON file to ensure persistence across sessions.
        public void SerializeSaveTasks()
        {
            JsonManager.SerializeSaveTasks(SaveTasks);
        }
    }
}
