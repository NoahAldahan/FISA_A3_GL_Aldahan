using System;
using System.Collections.Generic;
using Log;
using EasySaveWeb.Utilities;
using System.IO;

namespace EasySaveWeb.Model
{
    // Enum representing the types of save tasks available.
    public enum ESaveTaskTypes
    {
        Differential = 1, // Represents a differential backup (only modified files).
        Complete = 2 // Represents a complete backup (copies all files).
    }

    // Factory class responsible for creating instances of different save tasks.
    internal class SaveTaskFactory
    {
        // Logging instances for real-time and daily logs.
        private LogRealTime logRealTime;
        private LogDaily logDaily;

        // Creates a new save task of the specified type with the given source and target paths.
        internal SaveTask CreateSave(ESaveTaskTypes saveTaskTypes, string sourcePath, string targetPath, string saveTaskName)
        {
            SaveTask saveTask;

            // Determine the type of save task to create.
            switch (saveTaskTypes)
            {
                case ESaveTaskTypes.Differential:
                    // Creates a differential backup task, which only saves modified files.
                    saveTask = new SaveTaskDifferential(
                        new DirectoryPair(sourcePath, targetPath),
                        new LogDaily(JsonManager.LogPathDaily, JsonManager.LogPathRealTime),
                        new LogRealTime(JsonManager.LogPathDaily, JsonManager.LogPathRealTime),
                        saveTaskName);
                    return saveTask;

                case ESaveTaskTypes.Complete:
                    saveTask = new SaveTaskComplete(new DirectoryPair(sourcePath, targetPath), new LogDaily(JsonManager.LogPathDaily, JsonManager.LogPathRealTime), new LogRealTime(JsonManager.LogPathDaily, JsonManager.LogPathRealTime), saveTaskName);
                    return saveTask;

                default:
                    // Throws an exception if an invalid save task type is provided.
                    throw new ArgumentException("Invalid save task type");
            }
        }
    }
}
