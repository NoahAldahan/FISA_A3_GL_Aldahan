using System;

namespace EasySaveConsole.Model
{
    public enum SaveTaskTypes
    {
        Differential,
        Complete
    }
    public class SaveTaskFactory
	{
        public SaveTask CreateSave(SaveTaskTypes saveTaskTypes, string sourcePath, string targetPath)
        {
            switch (saveTaskTypes)
            {
                case SaveTaskTypes.Differential:
                    return new DifferentialSave(DirectoryPair(sourcePath, targetPath));
                case SaveTaskTypes.Complete:
                    return new CompleteSave(DirectoryPair(sourcePath, targetPath));
                default:
                    throw new ArgumentException("Invalid save task type");
            }
        }

	}
}