using System;
using System.Collections.Generic;
using EasySaveConsole.Model.Log;

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
            task.AddObserver(new LogDaily());
            task.AddObserver(new LogRealTime());
        }

        // Create a new save task of type saveTaskTypes with sourcePath and targetPath
        internal SaveTask CreateSave(ESaveTaskTypes saveTaskTypes, string sourcePath, string targetPath)
        {
            SaveTask saveTask;
            LogDaily logDaily = new LogDaily();
            LogRealTime logRealTime = new LogRealTime();
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