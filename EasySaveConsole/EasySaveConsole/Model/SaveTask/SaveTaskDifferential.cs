using EasySaveConsole.Model.Log;
using Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EasySaveConsole.Model
{
    // Class representing a differential save task, inheriting from SaveTask.
    internal class SaveTaskDifferential : SaveTask
    {
        // Constructor for initializing the task with only a directory pair and name.
        [JsonConstructor]
        internal SaveTaskDifferential(DirectoryPair CurrentDirectoryPair, string name)
            : base(CurrentDirectoryPair, name) { }

        // Constructor for initializing the task with directory pair, logs, and name.
        internal SaveTaskDifferential(DirectoryPair CurrentDirectoryPair, LogDaily logDaily, LogRealTime logRealTime, string SaveTaskName, LogManager logManager)
            : base(CurrentDirectoryPair, logDaily, logRealTime, SaveTaskName, logManager) { }

        // Starts a differential save task.
        // Returns true if all files were saved successfully, false otherwise.
        // To get the paths of all the files and directories unsaved, call GetUnsavedPaths().
        internal override bool Save()
        {
            logDaily.CreateDailyFile((int)logManager.defaultSaveTaskType);
            logRealTime.CreateRealTimeInfo(name, CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath, ERealTimeState.ACTIVE, (int)ESaveTaskTypes.Differential, (int)logManager.defaultSaveTaskType);
            UnsavedPaths.Clear();
            UnsavedPaths = SaveDifferentialRecursive(CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath);
            return (UnsavedPaths.Count() == 0);
        }

        // Recursively saves only the updated files and directories since the last save.
        private List<string> SaveDifferentialRecursive(string SourcePath, string TargetPath)
        {
            try
            {
                // Retrieve the file attributes to determine if the source is a file or directory.
                FileAttributes sourceAttr = File.GetAttributes(SourcePath);

            // Case 1: The source is a file
            if (!sourceAttr.HasFlag(FileAttributes.Directory))
            {
                FileInfo sourceFileInfo = new FileInfo(SourcePath);
                FileInfo targetFileInfo = new FileInfo(Path.Combine(TargetPath, sourceFileInfo.Name));
                // If the file doesn't exist or the source file is more recent than the target file
                // We use targetFileInfo.FullName instead of TargetPath because we need the full path of the file
                // (with the name of the file appended) that is going to be created or updated
                DateTime lastSaveDateJson = JsonLogManager.GetLastSaveDateFromJson(logDaily.LogDailyPath, sourceFileInfo.FullName);
                DateTime lastSaveDateXml = XmlLogManager.GetLastSaveDateFromXml(logDaily.LogDailyPath, sourceFileInfo.FullName);
                DateTime lastSaveDate = lastSaveDateJson > lastSaveDateXml ? lastSaveDateJson : lastSaveDateXml;
                if (!File.Exists(targetFileInfo.FullName) || sourceFileInfo.LastWriteTime > lastSaveDate)
                {
                    logDaily.stopWatch.Restart();
                    File.Copy(SourcePath, targetFileInfo.FullName, true);
                    logDaily.stopWatch.Stop();
                    logDaily.AddDailyInfo(name, SourcePath, targetFileInfo.FullName, (int)logManager.defaultSaveTaskType);
                    logRealTime.UpdateRealTimeProgress((int)logManager.defaultSaveTaskType);
                }
            }
            // Case 2: The source is a directory
            else
            {
                DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(SourcePath);
                DirectoryInfo targetDirectoryInfo = new DirectoryInfo(TargetPath);

                    // If the source directory is empty and the target directory does not exist, create the target directory.
                    if (sourceDirectoryInfo.GetDirectories().Length == 0 && !targetDirectoryInfo.Exists)
                    {
                        targetDirectoryInfo.Create();
                    }
                    else
                    {
                        // Iterate through all subdirectories and process them recursively.
                        foreach (DirectoryInfo dir in sourceDirectoryInfo.GetDirectories())
                        {
                            SaveDifferentialRecursive(dir.FullName, Path.Combine(targetDirectoryInfo.FullName, dir.Name));
                        }
                        foreach (FileInfo file in sourceDirectoryInfo.GetFiles())
                        {
                            SaveDifferentialRecursive(file.FullName, targetDirectoryInfo.FullName);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UnsavedPaths.Add(SourcePath);
            }
            
            return UnsavedPaths;
        }

        internal override EMessage GetMessageSaveTaskType()
        {
            return EMessage.SaveTaskTypeDifferentialName;
        }
        internal override ESaveTaskTypes GetSaveTaskType()
        {
            return ESaveTaskTypes.Differential;
        }
    }
}

