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
        internal SaveTaskDifferential(DirectoryPair CurrentDirectoryPair, LogDaily logDaily, LogRealTime logRealTime, string SaveTaskName)
            : base(CurrentDirectoryPair, logDaily, logRealTime, SaveTaskName) { }

        // Starts a differential save task.
        internal override bool Save()
        {
            // TODO: Improve error handling to specify files that couldn't be saved.
            IsSaveSuccessful = true;
            try
            {
                // Initiates the differential backup process recursively.
                SaveDifferentialRecursive(CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath);
            }
            catch (Exception ex)
            {
                IsSaveSuccessful = false; // Marks the backup as unsuccessful if an error occurs.
            }
            return IsSaveSuccessful;
        }

        // Recursively saves only the updated files and directories since the last save.
        private void SaveDifferentialRecursive(string SourcePath, string TargetPath)
        {
            // Retrieve the file attributes to determine if the source is a file or directory.
            FileAttributes sourceAttr = File.GetAttributes(SourcePath);

            // Case 1: The source is a file
            if (!sourceAttr.HasFlag(FileAttributes.Directory))
            {
                FileInfo sourceFileInfo = new FileInfo(SourcePath);
                FileInfo targetFileInfo = new FileInfo(Path.Combine(TargetPath, sourceFileInfo.Name));

                // If the file does not exist in the target directory or if the source file is more recent than the last saved file.
                // TEMP: Replace `targetFileInfo.LastWriteTime` with value retrieved from logs for better accuracy.
                if (!File.Exists(targetFileInfo.FullName) ||
                    sourceFileInfo.LastWriteTime > JsonLogManager.GetLastSaveDate(this.logDaily.LogDailyPath, sourceFileInfo.FullName))
                {
                    // Copy the updated file to the target directory.
                    File.Copy(SourcePath, targetFileInfo.FullName, true);
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

                    // Iterate through all files in the current directory and process them recursively.
                    foreach (FileInfo file in sourceDirectoryInfo.GetFiles())
                    {
                        SaveDifferentialRecursive(file.FullName, targetDirectoryInfo.FullName);
                    }
                }
            }
        }
    }
}

