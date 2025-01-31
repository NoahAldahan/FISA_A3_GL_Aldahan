using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text.Json;

namespace EasySaveConsole.Model
{
	public class SaveTaskManager
	{
        // All the current save tasks
        public List<SaveTask> SaveTasks { get; set; }
        // The save task factory to create new save tasks
        public SaveTaskFactory SaveTaskFactory { get; set; }
        // The maximum number of save tasks that can be created at once
        private static int MaxSaveTasks = 5;

        // Getter for the save tasks
        public List<SaveTask> GetSaveTasksClone()
        {
            return new List<SaveTask>(SaveTasks);
        }

        // Constructor
        public SaveTaskManager()
        {
            SaveTaskFactory = new SaveTaskFactory();
            SaveTasks = new List<SaveTask>();
            // Load the save tasks that are saved from previous session
            DeserializeSaveTasks();
        }

        // Add a new save task of type SaveTaskType with sourcePath and targetPath
        public void AddSaveTask(ESaveTaskTypes SaveTaskType, string sourcePath, string targetPath)
        {
            if (SaveTasks.Count >= MaxSaveTasks)
            {
                throw new Exception("Maximum number of save tasks reached");
            }
            SaveTasks.Add(SaveTaskFactory.CreateSave(SaveTaskType, sourcePath, targetPath));
        }

        // Remove a save task at index
        public void RemoveSaveTask(int index)
        {
            SaveTasks.RemoveAt(index);
        }

        // Remove a save task of type SaveTaskType with sourcePath and targetPath (stops after the first deletion)
        public void RemoveSaveTask(ESaveTaskTypes SaveTaskType, string sourcePath, string targetPath)
        {
            foreach (SaveTask saveTask in SaveTasks)
            {
                if (saveTask.CurrentDirectoryPair.SourcePath == sourcePath && saveTask.CurrentDirectoryPair.TargetPath == targetPath)
                {
                    SaveTasks.Remove(saveTask);
                    break;
                }
            }
        }

        // Starts the save task at index
        public void ExecuteSaveTask(int index)
        {
            SaveTasks[index].Save();
        }

        // Starts all save tasks
        public void ExecuteAllSaveTasks()
        {
            foreach (SaveTask saveTask in SaveTasks)
            {
                saveTask.Save();
            }
        }

        // Saves all save tasks config to a json file for persistence
        public void SerializeSaveTasks()
        {
            //TEMP : we're writing in the json, but should use JSON manager
            string json = JsonSerializer.Serialize(SaveTasks);
            Console.WriteLine(json);
        }

        // Loads all save tasks config from a json file for persistence
        public void DeserializeSaveTasks()
        {
            //TEMP : we're reading from the json, but should use JSON manager
            string json = "";
            if(json == "")
            {
                return;
            }
            SaveTasks = JsonSerializer.Deserialize<List<SaveTask>>(json);

            //TEMP : writing to console for debugging purposes
            List<string> info = new List<string>();
            foreach (SaveTask saveTask in SaveTasks)
            {
                info.Clear();
                info = new List<string>(saveTask.GetInfo());
                foreach (string line in info)
                {
                    Console.WriteLine(line);
                }
            }
        }
    }
}