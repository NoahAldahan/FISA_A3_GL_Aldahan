using System;
using System.Collections.Generic;
using EasySaveConsole.Model.Log;
using EasySaveConsole.Utilities;

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


        // Create a new save task of type saveTaskTypes with sourcePath and targetPath
        internal SaveTask CreateSave(ESaveTaskTypes saveTaskTypes, string sourcePath, string targetPath)
        {
            SaveTask saveTask;
            JsonLogManager logManager= new JsonLogManager();
            LogRealTime logRealTime = new LogRealTime(logManager);
            LogDaily logDaily = new LogDaily(logManager);

            switch (saveTaskTypes)
            {
                case ESaveTaskTypes.Differential:
                    saveTask = new SaveTaskDifferential(new DirectoryPair(sourcePath, targetPath),new LogDaily(logManager), new LogRealTime(logManager));
                    return saveTask;
                case ESaveTaskTypes.Complete:
                    saveTask = new SaveTaskComplete(new DirectoryPair(sourcePath, targetPath), new LogDaily(logManager), new LogRealTime(logManager));
                    return saveTask;
                default:
                    throw new ArgumentException("Invalid save task type");
            }
            
        }

	}
}