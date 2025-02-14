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

namespace EasySaveConsole.Model
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
        internal abstract bool Save();
    }
}

