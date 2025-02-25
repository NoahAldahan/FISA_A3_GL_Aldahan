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
using System.Threading;

namespace EasySaveWPFApp.Model
{
    // Class representing a complete save task, inheriting from SaveTask.
    internal class SaveTaskComplete : SaveTask
    {
        // Constructor with a directory pair and task name.
        [JsonConstructor]
        internal SaveTaskComplete(DirectoryPair CurrentDirectoryPair, string name)
            : base(CurrentDirectoryPair, name) { }

        // Constructor with directory pair and log instances.
        internal SaveTaskComplete(DirectoryPair CurrentDirectoryPair, LogDaily logDaily, LogRealTime logRealTime, string saveTaskName)
            : base(CurrentDirectoryPair, logDaily, logRealTime, saveTaskName) { }

        // Overrides the abstract Save method to perform a complete backup.
        // Returns true if all files were saved successfully, false otherwise.
        // To get the paths of all the files and directories unsaved, call GetUnsavedPaths().
        internal override bool Save(SaveTaskManager saveTaskManager)
        {
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
                Trace.WriteLine("save comp save(stm) Exception e");
                UnsavedPaths.Add(CurrentDirectoryPair.SourcePath);
                return false;
            }
            UnsavedPaths = SaveComplete(saveTaskManager); // Perform the complete save process.
            if (state != ERealTimeState.STOPPED && state != ERealTimeState.WAITING_FOR_PRIORITY_FILES) SetBindState(ERealTimeState.END);

            Trace.WriteLine("EndSave");
            return (UnsavedPaths.Count() == 0);
        }

        // Performs the complete backup by copying files from source to target.
        private List<string> SaveComplete(SaveTaskManager saveTaskManager)
        {
            try
            {
                // Get file attributes to determine if the source and target are directories or files.
                FileAttributes sourceAttr = File.GetAttributes(CurrentDirectoryPair.SourcePath);
                FileAttributes targetAttr = File.GetAttributes(CurrentDirectoryPair.TargetPath);

                // Case 1: Both source and target are directories
                if (sourceAttr.HasFlag(FileAttributes.Directory) && targetAttr.HasFlag(FileAttributes.Directory) && !isSoftwareRunning)
                {
                    DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(CurrentDirectoryPair.SourcePath);
                    DirectoryInfo targetDirectoryInfo = new DirectoryInfo(CurrentDirectoryPair.TargetPath);
                    CopyFilesRecursivelyForTwoFolders(sourceDirectoryInfo, targetDirectoryInfo, saveTaskManager);
                }
                // Case 2: Source is a file, target is a directory
                else if (!sourceAttr.HasFlag(FileAttributes.Directory) && targetAttr.HasFlag(FileAttributes.Directory) && !isSoftwareRunning)
                {
                    string FileName = Path.GetFileName(CurrentDirectoryPair.SourcePath);

                    try
                    {
                        CopySingleFile(CurrentDirectoryPair.SourcePath, Path.Combine(CurrentDirectoryPair.TargetPath, FileName), saveTaskManager);
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine("save complete second Exception e");
                        UnsavedPaths.Add(CurrentDirectoryPair.SourcePath);
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("save complete first Exception e");
                UnsavedPaths.Add(CurrentDirectoryPair.SourcePath);
            }
            return UnsavedPaths;
        }

        // Recursively copies all files and subdirectories from the source to the target directory.
        private List<string> CopyFilesRecursivelyForTwoFolders(DirectoryInfo sourceDirectoryInfo, DirectoryInfo targetDirectoryInfo, SaveTaskManager saveTaskManager)
        {
            try
            {
                // Iterate through all directories in the source and create them in the target.
                foreach (DirectoryInfo dir in sourceDirectoryInfo.GetDirectories())
                {
                    if (isSoftwareRunning)
                    {
                        throw new Exception("Error unauthorized  software is running");
                    }
                    CopyFilesRecursivelyForTwoFolders(dir, targetDirectoryInfo.CreateSubdirectory(dir.Name), saveTaskManager);
                }
                foreach (FileInfo file in sourceDirectoryInfo.GetFiles())
                {
                    try
                    {
                        if (isSoftwareRunning)
                        {
                            throw new Exception("Error unauthorized  software is running");
                        }
                        CopySingleFile(file.FullName, Path.Combine(targetDirectoryInfo.FullName, file.Name), saveTaskManager);
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLine("save complete copy files recursively Is software running + copy single file");
                        UnsavedPaths.Add(Path.Combine(sourceDirectoryInfo.FullName, file.Name));
                    }
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("save complete CopyFilesRecursivelyForTwoFolders big try catch");
                UnsavedPaths.Add(sourceDirectoryInfo.FullName);
            }
            return UnsavedPaths;
        }
        internal override ESaveTaskTypes GetSaveTaskType()
        {
            return ESaveTaskTypes.Complete;
        }




        // TEST


        private List<string> TestCopyFilesRecursivelyForTwoFolders(DirectoryInfo sourceDirectoryInfo, DirectoryInfo targetDirectoryInfo, SaveTaskManager saveTaskManager)
        {
            try
            {
                List<string> priorityExtensions = saveTaskManager.GetPriorityExtensions(); // Get the priority extensions
                HashSet<string> prioritySet = new HashSet<string>(priorityExtensions, StringComparer.OrdinalIgnoreCase);

                // Iterate through all directories in the source and create them in the target.
                foreach (DirectoryInfo dir in sourceDirectoryInfo.GetDirectories())
                {
                    if (isSoftwareRunning)
                    {
                        throw new Exception("Error unauthorized software is running");
                    }
                    CopyFilesRecursivelyForTwoFolders(dir, targetDirectoryInfo.CreateSubdirectory(dir.Name), saveTaskManager);
                }

                // Get all files and sort them so that priority files are processed first
                List<FileInfo> allFiles = sourceDirectoryInfo.GetFiles().ToList();
                var priorityFiles = allFiles.Where(f => prioritySet.Contains(f.Extension)).ToList();
                var otherFiles = allFiles.Except(priorityFiles).ToList();

                // Copy priority files first
                foreach (FileInfo file in priorityFiles)
                {
                    TryCopyFile(file, targetDirectoryInfo, saveTaskManager);
                }

                // Copy remaining files
                foreach (FileInfo file in otherFiles)
                {
                    TryCopyFile(file, targetDirectoryInfo, saveTaskManager);
                }
            }
            catch (Exception e)
            {
                UnsavedPaths.Add(sourceDirectoryInfo.FullName);
            }
            return UnsavedPaths;
        }

        private void TryCopyFile(FileInfo file, DirectoryInfo targetDirectoryInfo, SaveTaskManager saveTaskManager)
        {
            try
            {
                if (isSoftwareRunning)
                {
                    throw new Exception("Error unauthorized software is running");
                }
                CopySingleFile(file.FullName, Path.Combine(targetDirectoryInfo.FullName, file.Name), saveTaskManager);
            }
            catch (Exception e)
            {
                UnsavedPaths.Add(file.FullName);
            }
        }
    }
}

