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
        Differential,
        Complete
    }
    internal class SaveTaskFactory
	{
        private LogRealTime logRealTime;
        private LogDaily logDaily;
        private string LogPathDaily;
        private string LogPathRealTime;

        internal SaveTaskFactory()
        {
            LogPathDaily = Path.Combine(Directory.GetCurrentDirectory(), "..", "..",
            Environment.GetEnvironmentVariable("LogPathDaily"));

            LogPathRealTime = Path.Combine(Directory.GetCurrentDirectory(), "..", "..",
            Environment.GetEnvironmentVariable("LogPathRealTime"));

            logRealTime = new LogRealTime(LogPathDaily, LogPathRealTime);
            logDaily = new LogDaily(LogPathDaily, LogPathRealTime);
        }


        // Create a new save task of type saveTaskTypes with sourcePath and targetPath
        internal SaveTask CreateSave(ESaveTaskTypes saveTaskTypes, string sourcePath, string targetPath)
        {
            SaveTask saveTask;

            switch (saveTaskTypes)
            {
                case ESaveTaskTypes.Differential:
                    saveTask = new SaveTaskDifferential(new DirectoryPair(sourcePath, targetPath), logDaily, logRealTime);
                    return saveTask;
                case ESaveTaskTypes.Complete:
                    saveTask = new SaveTaskComplete(new DirectoryPair(sourcePath, targetPath), logDaily, logRealTime);
                    return saveTask;
                default:
                    throw new ArgumentException("Invalid save task type");
            }
        }

	}
}