using Log;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EasySaveWPFApp.Model
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
        // Returns true if all files were saved successfully, false otherwise.
        // To get the paths of all the files and directories unsaved, call GetUnsavedPaths().
        internal override bool Save(SaveTaskManager saveTaskManager)
        {
            try
            {
                Trace.WriteLine("savediff starting try");
                UnsavedPaths = SaveDifferentialRecursive(CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath, saveTaskManager);
                nFilesUnsavedCancelled = logRealTime.GetTotalFilesLeftToDo();
                Trace.WriteLine("savediff finished try");
            }
            catch (Exception ex)
            {
                nFilesUnsavedCancelled = logRealTime.GetTotalFilesLeftToDo();
            }
            if (state != ERealTimeState.STOPPED && state != ERealTimeState.WAITING_FOR_PRIORITY_FILES
                && state != ERealTimeState.ERROR) SetBindState(ERealTimeState.END);
            return (UnsavedPaths.Count() == 0 && nFilesUnsavedCancelled == 0);
        }

        // Recursively saves only the updated files and directories since the last save.
        private List<string> SaveDifferentialRecursive(string SourcePath, string TargetPath, SaveTaskManager saveTaskManager)
        {
            try
            {
                Trace.WriteLine("savediffrec start");

                // Retrieve the file attributes to determine if the source is a file or directory.
                FileAttributes sourceAttr = File.GetAttributes(SourcePath);
                Trace.WriteLine("sourceAttr");

                // Case 1: The source is a file
                if (!sourceAttr.HasFlag(FileAttributes.Directory))
                {
                    FileInfo sourceFileInfo = new FileInfo(SourcePath);
                    Trace.WriteLine("sourceFileInfo");
                    FileInfo targetFileInfo = new FileInfo(Path.Combine(TargetPath, sourceFileInfo.Name));
                    Trace.WriteLine("targetFileInfo");
                    // If the file doesn't exist or the source file is more recent than the target file
                    // We use targetFileInfo.FullName instead of TargetPath because we need the full path of the file
                    // (with the name of the file appended) that is going to be created or updated
                    DateTime JSONLastDate = JsonLogManager.GetLastSaveDateFromJson(this.logDaily.LogDailyPath, sourceFileInfo.FullName);
                    DateTime XMLLastDate = XmlLogManager.GetLastSaveDateFromXml(this.logDaily.LogDailyPath, sourceFileInfo.FullName);
                    DateTime MostRecent = JSONLastDate > XMLLastDate ? JSONLastDate : XMLLastDate;

                    if (!File.Exists(targetFileInfo.FullName) || (sourceFileInfo.LastWriteTime > MostRecent))
                    {
                        Trace.WriteLine("before copy single file");
                        CopySingleFile(SourcePath, targetFileInfo.FullName, saveTaskManager);
                        Trace.WriteLine("after copy single file");
                    }
                }
                // Case 2: The source is a directory
                else
                {
                    Trace.WriteLine("else copy single file");
                    DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(SourcePath);
                    Trace.WriteLine("sourceDirectoryInfo");
                    DirectoryInfo targetDirectoryInfo = new DirectoryInfo(TargetPath);
                    Trace.WriteLine("sourceDirectoryInfo");

                    // If the source directory is empty and the target directory does not exist, create the target directory.
                    if (sourceDirectoryInfo.GetDirectories().Length == 0 && !targetDirectoryInfo.Exists)
                    {
                        Trace.WriteLine("before create directory");
                        CreateDirectory(TargetPath);
                        Trace.WriteLine("after create directory");
                    }
                    else
                    {
                        // Iterate through all subdirectories and process them recursively.
                        foreach (DirectoryInfo dir in sourceDirectoryInfo.GetDirectories())
                        {
                            Trace.WriteLine("before recursive call on folders");
                            SaveDifferentialRecursive(dir.FullName, Path.Combine(targetDirectoryInfo.FullName, dir.Name), saveTaskManager);
                            Trace.WriteLine("after recursive call on folders");
                        }
                        foreach (FileInfo file in sourceDirectoryInfo.GetFiles())
                        {
                            Trace.WriteLine("before recursive call on files");
                            SaveDifferentialRecursive(file.FullName, targetDirectoryInfo.FullName, saveTaskManager);
                            Trace.WriteLine("after recursive call on files");
                        }
                    }
                }
            }
            catch (OperationCanceledException opEx)
            {
                Trace.WriteLine("savediffrec OperationCanceledException opEx");
                throw opEx;
            }
            catch (Exception ex)
            {
                Trace.WriteLine("savediffrec Exception ex");
                UnsavedPaths.Add(SourcePath);
                SetBindState(ERealTimeState.ERROR);
            }
            Trace.WriteLine("savediff before update progress");
            UpdateProgress();
            Trace.WriteLine("savediff after update progress");
            return UnsavedPaths;
        }

        internal override ESaveTaskTypes GetSaveTaskType()
        {
            return ESaveTaskTypes.Differential;
        }


        // TEST 


        private List<string> TestSaveDifferentialRecursive(string SourcePath, string TargetPath, SaveTaskManager saveTaskManager)
        {
            try
            {
                List<string> priorityExtensions = saveTaskManager.GetPriorityExtensions(); // Get the priority extensions
                HashSet<string> prioritySet = new HashSet<string>(priorityExtensions, StringComparer.OrdinalIgnoreCase);

                FileAttributes sourceAttr = File.GetAttributes(SourcePath);

                if (!sourceAttr.HasFlag(FileAttributes.Directory))
                {
                    FileInfo sourceFileInfo = new FileInfo(SourcePath);
                    FileInfo targetFileInfo = new FileInfo(Path.Combine(TargetPath, sourceFileInfo.Name));

                    if (!File.Exists(targetFileInfo.FullName)
                        || sourceFileInfo.LastWriteTime > JsonLogManager.GetLastSaveDateFromJson(this.logDaily.LogDailyPath, sourceFileInfo.FullName)
                        || sourceFileInfo.LastWriteTime > XmlLogManager.GetLastSaveDateFromXml(this.logDaily.LogDailyPath, sourceFileInfo.FullName))
                    {
                        CopySingleFile(SourcePath, targetFileInfo.FullName, saveTaskManager);
                    }
                }
                else
                {
                    DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(SourcePath);
                    DirectoryInfo targetDirectoryInfo = new DirectoryInfo(TargetPath);

                    if (sourceDirectoryInfo.GetDirectories().Length == 0 && !targetDirectoryInfo.Exists)
                    {
                        targetDirectoryInfo.Create();
                    }
                    else
                    {
                        foreach (DirectoryInfo dir in sourceDirectoryInfo.GetDirectories())
                        {
                            SaveDifferentialRecursive(dir.FullName, Path.Combine(targetDirectoryInfo.FullName, dir.Name), saveTaskManager);
                        }

                        List<FileInfo> allFiles = sourceDirectoryInfo.GetFiles().ToList();
                        var priorityFiles = allFiles.Where(f => prioritySet.Contains(f.Extension)).ToList();
                        var otherFiles = allFiles.Except(priorityFiles).ToList();

                        foreach (FileInfo file in priorityFiles)
                        {
                            SaveDifferentialRecursive(file.FullName, targetDirectoryInfo.FullName, saveTaskManager);
                        }

                        foreach (FileInfo file in otherFiles)
                        {
                            SaveDifferentialRecursive(file.FullName, targetDirectoryInfo.FullName, saveTaskManager);
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
    }
}

