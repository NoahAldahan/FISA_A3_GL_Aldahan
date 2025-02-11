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
    [JsonDerivedType(typeof(SaveTaskComplete), "SaveTaskComplete")]
    [JsonDerivedType(typeof(SaveTaskDifferential), "SaveTaskDifferential")]
    internal abstract class SaveTask
    {
        // The directory name storing the target and source directories
        [JsonInclude]
        internal DirectoryPair CurrentDirectoryPair { get; set; }

        protected bool IsSaveSuccessful;
        internal LogRealTime logRealTime;
        internal LogDaily logDaily;

        [JsonInclude]
        internal string name;

        internal void SetLogRealTime(LogRealTime logRealTime)
        {
            this.logRealTime = logRealTime;
        }

        internal void SetLogDaily(LogDaily logDaily)
        {
            this.logDaily = logDaily;
        }


        // Constructor
        [JsonConstructor]
        internal SaveTask(DirectoryPair CurrentDirectoryPair, string name)
        {
            this.CurrentDirectoryPair = CurrentDirectoryPair;
            this.name = name;
        }

        internal SaveTask(DirectoryPair CurrentDirectoryPair, LogDaily logDaily, LogRealTime logRealTime, string saveTaskName)
        {
            this.CurrentDirectoryPair = CurrentDirectoryPair;
            this.logDaily = logDaily;
            this.logRealTime = logRealTime;
            this.name = saveTaskName;
        }

        // Get the directory pair
        internal DirectoryPair GetDirectoryPair()
        {
            return CurrentDirectoryPair;
        }

        // Start the task
        // Returns true if the task was successful (all files were saved), false otherwise
        internal abstract bool Save();
    }
}
