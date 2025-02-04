using System;

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
            switch (saveTaskTypes)
            {
                case ESaveTaskTypes.Differential:
                    return new SaveTaskDifferential(new DirectoryPair(sourcePath, targetPath));
                case ESaveTaskTypes.Complete:
                    return new SaveTaskComplete(new DirectoryPair(sourcePath, targetPath));
                default:
                    throw new ArgumentException("Invalid save task type");
            }
        }

	}
}