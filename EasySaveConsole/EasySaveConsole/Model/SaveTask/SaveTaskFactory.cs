using System;
using System.Collections.Generic;
using Log;
using EasySaveConsole.Utilities;
using System.IO;

namespace EasySaveConsole.Model
{
    // The types of save tasks
    public enum ESaveTaskTypes
    {
        Differential = 1,
        Complete = 2
    }
    internal class SaveTaskFactory
	{
        private LogRealTime logRealTime;
        private LogDaily logDaily;

        // Create a new save task of type saveTaskTypes with sourcePath and targetPath
        internal SaveTask CreateSave(ESaveTaskTypes saveTaskTypes, string sourcePath, string targetPath, string saveTaskName)
        {
            SaveTask saveTask;

            switch (saveTaskTypes)
            {
                case ESaveTaskTypes.Differential:
                    saveTask = new SaveTaskDifferential(new DirectoryPair(sourcePath, targetPath), new LogDaily(JsonManager.LogPathDaily, JsonManager.LogPathRealTime), new LogRealTime(JsonManager.LogPathDaily, JsonManager.LogPathRealTime), saveTaskName);
                    return saveTask;
                case ESaveTaskTypes.Complete:
                    saveTask = new SaveTaskComplete(new DirectoryPair(sourcePath, targetPath), logDaily, logRealTime, saveTaskName);
                    return saveTask;
                default:
                    throw new ArgumentException("Invalid save task type");
            }
        }
    }
}