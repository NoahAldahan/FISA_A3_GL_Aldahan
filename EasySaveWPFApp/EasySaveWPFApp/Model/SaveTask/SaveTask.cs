using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using Log;
using System.Threading.Tasks;
using EasySaveWPFApp.Utilities;

namespace EasySaveWPFApp.Model
{
    // Specifies that the class can be serialized as a derived type in JSON format.
    [JsonDerivedType(typeof(SaveTaskComplete), "SaveTaskComplete")]
    [JsonDerivedType(typeof(SaveTaskDifferential), "SaveTaskDifferential")]
    internal abstract class SaveTask
    {
        // Stores the source and target directory pair for the backup task.
        [JsonInclude]
        internal DirectoryPair CurrentDirectoryPair { get; set; }

        // Boolean flag to track whether the save operation was successful.
        protected bool IsSaveSuccessful;

        // Logs for real-time and daily backup operations.
        internal LogRealTime logRealTime;
        internal LogDaily logDaily;
        //
        internal List<string> UnsavedPaths;

        // Name of the backup task.
        [JsonInclude]
        internal string name;

        // Setter for the real-time logging instance.
        internal void SetLogRealTime(LogRealTime logRealTime)
        {
            this.logRealTime = logRealTime;
        }

        // Setter for the daily logging instance.
        internal void SetLogDaily(LogDaily logDaily)
        {
            this.logDaily = logDaily;
        }

        // Setter for the daily logging instance.
        internal List<string> GetUnsavedPaths()
        {
            return new List<string>(UnsavedPaths);
        }

        // Constructor for the SaveTask class with a directory pair and task name.
        [JsonConstructor]
        internal SaveTask(DirectoryPair CurrentDirectoryPair, string name)
        {
            this.CurrentDirectoryPair = CurrentDirectoryPair;
            this.name = name;
            this.UnsavedPaths = new List<string>();
        }

        // Overloaded constructor with additional parameters for logging instances.
        internal SaveTask(DirectoryPair CurrentDirectoryPair, LogDaily logDaily, LogRealTime logRealTime, string saveTaskName)
        {
            this.CurrentDirectoryPair = CurrentDirectoryPair;
            this.logDaily = logDaily;
            this.logRealTime = logRealTime;
            this.name = saveTaskName;
            this.UnsavedPaths = new List<string>();
        }

        // Returns the directory pair associated with the backup task.
        internal DirectoryPair GetDirectoryPair()
        {
            return CurrentDirectoryPair;
        }

        internal abstract EMessage GetMessageSaveTaskType();
        internal abstract ESaveTaskTypes GetSaveTaskType();

        // Start the task
        // Returns true if the task was successful (all files were saved), false otherwise
        // To get the paths of all the files and directories unsaved, call GetUnsavedPaths().
        internal abstract bool Save(List<string> EncryptingExtensions);

        // Method that will be called when a single file is copied (called by recursive and non-recursives)
        // It also handles log calls when a file is copied
        // Can throw unauthorized access exception
        internal void CopySingleFile(string sourcePath, string targetPath, List<string> EncryptingExtensions)
        {
            logDaily.stopWatch.Restart(); // Start timing the copy operation.
            File.Copy(sourcePath, targetPath, true);// Copy the file from source to target directory.
            logDaily.stopWatch.Stop();
            //notify save of a new file
            logDaily.AddDailyInfo(name, CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath);
            logRealTime.UpdateRealTimeProgress();

            // Encrypt the file if it has an extension that requires encryption.
            if (EncryptingExtensions.Contains(Path.GetExtension(targetPath)))
                CryptoSoftLibrary.CryptoSoftLibrary.EncryptFile(targetPath, JsonManager.EncryptionKey); 
        }
    }
}

