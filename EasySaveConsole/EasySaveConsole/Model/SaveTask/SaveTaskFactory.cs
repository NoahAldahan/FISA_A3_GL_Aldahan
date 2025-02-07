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

        internal void InitObserver(SaveTask task)
        {
            JsonLogManager jsonLogManager = new JsonLogManager();
            task.AddObserver(new LogDaily(jsonLogManager));
            task.AddObserver(new LogRealTime(jsonLogManager));
        }

        // Create a new save task of type saveTaskTypes with sourcePath and targetPath
        internal SaveTask CreateSave(ESaveTaskTypes saveTaskTypes, string sourcePath, string targetPath)
        {
            SaveTask saveTask;
            switch (saveTaskTypes)
            {
                case ESaveTaskTypes.Differential:
                    saveTask = new SaveTaskDifferential(new DirectoryPair(sourcePath, targetPath));
                    InitObserver(saveTask);
                    return saveTask;
                case ESaveTaskTypes.Complete:
                    saveTask = new SaveTaskComplete(new DirectoryPair(sourcePath, targetPath));
                    InitObserver(saveTask);
                    return saveTask;
                default:
                    throw new ArgumentException("Invalid save task type");
            }
            
        }

	}
}