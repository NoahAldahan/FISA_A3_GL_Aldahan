using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Log;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using EasySaveConsole.Model.Log;

namespace EasySaveConsole.Model
{
    // Class representing a complete save task, inheriting from SaveTask.
    internal class SaveTaskComplete : SaveTask
    {
        // Constructor with a directory pair and task name.
        [JsonConstructor]
        internal SaveTaskComplete(DirectoryPair CurrentDirectoryPair, string name)
            : base(CurrentDirectoryPair, name) { }

        // Constructor with directory pair and log instances.
        internal SaveTaskComplete(DirectoryPair CurrentDirectoryPair, LogDaily logDaily, LogRealTime logRealTime, string saveTaskName, LogManager logManager)
            : base(CurrentDirectoryPair, logDaily, logRealTime, saveTaskName, logManager) { }

        // Overrides the abstract Save method to perform a complete backup.
        // Returns true if all files were saved successfully, false otherwise.
        // To get the paths of all the files and directories unsaved, call GetUnsavedPaths().
        internal override bool Save()
        {
            UnsavedPaths.Clear(); // Clear the list of unsaved paths.
            UnsavedPaths = SaveComplete(); // Perform the complete save process.
            try
            {
                FileAttributes targetAttr = File.GetAttributes(CurrentDirectoryPair.TargetPath);
                if (!targetAttr.HasFlag(FileAttributes.Directory))
                {
                    UnsavedPaths.Add(CurrentDirectoryPair.SourcePath);
                    return false;
                }
            }
            catch (Exception e)
            {
                UnsavedPaths.Add(CurrentDirectoryPair.SourcePath);
                return false;
            }

            return (UnsavedPaths.Count() == 0);
        }

        // Performs the complete backup by copying files from source to target.
        private List<string> SaveComplete()
        {
            logDaily.CreateDailyFile((int)logManager.defaultSaveTaskType);
            logRealTime.CreateRealTimeInfo(name, CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath, ERealTimeState.ACTIVE, (int)ESaveTaskTypes.Complete,(int)logManager.defaultSaveTaskType);


            try
            {
                // Get file attributes to determine if the source and target are directories or files.
                FileAttributes sourceAttr = File.GetAttributes(CurrentDirectoryPair.SourcePath);
                FileAttributes targetAttr = File.GetAttributes(CurrentDirectoryPair.TargetPath);

                // Case 1: Both source and target are directories
                if (sourceAttr.HasFlag(FileAttributes.Directory) && targetAttr.HasFlag(FileAttributes.Directory))
                {
                    DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(CurrentDirectoryPair.SourcePath);
                    DirectoryInfo targetDirectoryInfo = new DirectoryInfo(CurrentDirectoryPair.TargetPath);
                    CopyFilesRecursivelyForTwoFolders(sourceDirectoryInfo, targetDirectoryInfo);
                }
                // Case 2: Source is a file, target is a directory
                else if (!sourceAttr.HasFlag(FileAttributes.Directory) && targetAttr.HasFlag(FileAttributes.Directory))
                {
                    logDaily.stopWatch.Restart(); // Start timing the copy operation.
                    string FileName = Path.GetFileName(CurrentDirectoryPair.SourcePath);

                    try
                    {
                        // Copy the file from source to target directory.
                        File.Copy(CurrentDirectoryPair.SourcePath, Path.Combine(CurrentDirectoryPair.TargetPath, FileName), true);
                    }
                    catch (Exception e)
                    {
                        UnsavedPaths.Add(CurrentDirectoryPair.SourcePath);
                    }
                    logDaily.stopWatch.Stop();
                    //notify save of a new file
                    logDaily.AddDailyInfo(name, CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath, (int)logManager.defaultSaveTaskType);
                    logRealTime.UpdateRealTimeProgress((int)logManager.defaultSaveTaskType);
                }
            }
            catch (Exception e)
            {
                UnsavedPaths.Add(CurrentDirectoryPair.SourcePath);
            }
            return UnsavedPaths;
        }

        // Recursively copies all files and subdirectories from the source to the target directory.
        private List<string> CopyFilesRecursivelyForTwoFolders(DirectoryInfo sourceDirectoryInfo, DirectoryInfo targetDirectoryInfo)
        {
            try
            {
                // Iterate through all directories in the source and create them in the target.
                foreach (DirectoryInfo dir in sourceDirectoryInfo.GetDirectories())
                    CopyFilesRecursivelyForTwoFolders(dir, targetDirectoryInfo.CreateSubdirectory(dir.Name));
                foreach (FileInfo file in sourceDirectoryInfo.GetFiles())
                {
                    try
                    {
                        logDaily.stopWatch.Restart(); // Start timing the file copy.

                        // Copy file to the corresponding target directory.
                        file.CopyTo(Path.Combine(targetDirectoryInfo.FullName, file.Name), true);
                        logDaily.stopWatch.Stop();
                        //notify save of a new file
                        logDaily.AddDailyInfo(name, file.FullName, targetDirectoryInfo.FullName + "\\" + file.Name, (int)logManager.defaultSaveTaskType);
                        logRealTime.UpdateRealTimeProgress((int)logManager.defaultSaveTaskType);
                    }
                    catch (Exception e)
                    {
                        UnsavedPaths.Add(Path.Combine(sourceDirectoryInfo.FullName, file.Name));
                    }
                }
            }
            catch (Exception e)
            {
                UnsavedPaths.Add(sourceDirectoryInfo.FullName);
            }
            return UnsavedPaths;
        }

        internal override EMessage GetMessageSaveTaskType()
        {
            return EMessage.SaveTaskTypeCompleteName;
        }
        internal override ESaveTaskTypes GetSaveTaskType()
        {
            return ESaveTaskTypes.Complete;
        }
    }
}

