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
                UnsavedPaths.Clear();
                Trace.WriteLine("savediff starting try");
                SaveDifferentialRecursive(CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath, saveTaskManager);
                nFilesUnsavedCancelled = logRealTime.realTimeInfo.NbFilesLeftToDo;
                Trace.WriteLine("savediff finished try");
            }
            catch (Exception ex)
            {
                Trace.WriteLine("savediff save exception");
                nFilesUnsavedCancelled = logRealTime.realTimeInfo.NbFilesLeftToDo;
            }
            if (state != ERealTimeState.STOPPED && state != ERealTimeState.WAITING_FOR_PRIORITY_FILES
                && state != ERealTimeState.ERROR) SetBindState(ERealTimeState.END);
            Trace.WriteLine("UnsavedPaths.Count() = " + UnsavedPaths.Count() + " nFilesUnsavedCancelled = "+ nFilesUnsavedCancelled);
            Trace.WriteLine("(UnsavedPaths.Count() == 0 && nFilesUnsavedCancelled == 0) = " + (UnsavedPaths.Count() == 0 && nFilesUnsavedCancelled == 0).ToString());
            UpdateProgress();
            if (state == ERealTimeState.WAITING_FOR_PRIORITY_FILES) return UnsavedPaths.Count() == 0;
            else return (UnsavedPaths.Count() == 0 && nFilesUnsavedCancelled == 0);
        }

        // Recursively saves only the updated files and directories since the last save.
        private void SaveDifferentialRecursive(string SourcePath, string TargetPath, SaveTaskManager saveTaskManager)
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
                    Trace.WriteLine(MostRecent.ToString());

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
                    if (!targetDirectoryInfo.Exists)
                    {
                        Trace.WriteLine("before create directory");
                        CreateDirectory(TargetPath);
                        Trace.WriteLine("after create directory");
                    }
                    // Iterate through all subdirectories and process them recursively.
                    foreach (DirectoryInfo dir in sourceDirectoryInfo.GetDirectories())
                    {
                        Trace.WriteLine("before recursive call on folders, new source : "+dir.FullName+"\n new target : "+ Path.Combine(targetDirectoryInfo.FullName, dir.Name));
                        SaveDifferentialRecursive(dir.FullName, Path.Combine(targetDirectoryInfo.FullName, dir.Name), saveTaskManager);
                        Trace.WriteLine("after recursive call on folders");
                    }
                    foreach (FileInfo file in sourceDirectoryInfo.GetFiles())
                    {
                        Trace.WriteLine("before recursive call on files, new source : " + file.FullName + "\n new target : " + targetDirectoryInfo.FullName);
                        SaveDifferentialRecursive(file.FullName, targetDirectoryInfo.FullName, saveTaskManager);
                        Trace.WriteLine("after recursive call on files");
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
                Trace.WriteLine("savediffrec Exception ex : " + ex.Message);
                UnsavedPaths.Add(SourcePath);
                SetBindState(ERealTimeState.ERROR);
            }
            Trace.WriteLine("SaveDifferentialRecursive before end");
        }

        internal override ESaveTaskTypes GetSaveTaskType()
        {
            return ESaveTaskTypes.Differential;
        }
    }
}

