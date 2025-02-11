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
        internal SaveTaskManager()
        {
            SaveTaskFactory = new SaveTaskFactory();
            SaveTasks = new List<SaveTask>();
            // Load the save tasks that are saved from previous session
            //DeserializeSaveTasks();
        }

        // Add a new save task of type SaveTaskType with sourcePath and targetPath
        internal void AddSaveTask(ESaveTaskTypes SaveTaskType, string sourcePath, string targetPath, string saveTaskName)
        {
            if (SaveTasks.Count >= MaxSaveTasks)
            {
                throw new Exception("Maximum number of save tasks reached");
            }
            SaveTasks.Add(SaveTaskFactory.CreateSave(SaveTaskType, sourcePath, targetPath, saveTaskName));
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


        // Starts all save tasks
        internal void ExecuteAllSaveTasks()
        {
            foreach (SaveTask saveTask in SaveTasks)
            {
                saveTask.Save();
            }
        }

        internal List<SaveTask> GetAllSaveTask()
        {
            return SaveTasks; 
        }

        // Modify the save task type
        internal void ModifySaveTaskType(int index, ESaveTaskTypes newSaveTaskType, string saveTaskName)
        {
            SaveTask newSaveTask = SaveTaskFactory.CreateSave(newSaveTaskType, SaveTasks[index].CurrentDirectoryPair.SourcePath, SaveTasks[index].CurrentDirectoryPair.TargetPath, saveTaskName);
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
        internal void SerializeSaveTasks()
        {
            //TEMP : we're writing in the json, but should use JSON manager
            string json = JsonSerializer.Serialize(SaveTasks);
            Console.WriteLine(json);
        }

        // Loads all save tasks config from a json file for persistence
        //internal void DeserializeSaveTasks()
        //{
        //    //TEMP : we're reading from the json, but should use JSON manager
        //    string json = "";
        //    if(json == "")
        //    {
        //        return;
        //    }
        //    SaveTasks = JsonSerializer.Deserialize<List<SaveTask>>(json);

        //    //TEMP : writing to console for debugging purposes
        //    List<string> info = new List<string>();
        //    foreach (SaveTask saveTask in SaveTasks)
        //    {
        //        info.Clear();
        //        info = new List<string>(saveTask.GetInfo());
        //        foreach (string line in info)
        //        {
        //            Console.WriteLine(line);
        //        }
        //    }
        //}
    }
}