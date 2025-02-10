using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EasySaveConsole.Model.Log;

namespace EasySaveConsole.Model
{
    [JsonDerivedType(typeof(SaveTaskComplete), "SaveTaskComplete")]
    [JsonDerivedType(typeof(SaveTaskDifferential), "SaveTaskDifferential")]
    internal abstract class SaveTask
    {
        // The directory name storing the target and source directories
        [JsonInclude]
        internal DirectoryPair CurrentDirectoryPair { get; set; }
        internal List<Log.Log> LogObserver { get; set; }

        internal LogRealTime logRealTime;
        internal LogDaily logDaily;
        internal string name;




        // Constructor
        [JsonConstructor]
        internal SaveTask(DirectoryPair CurrentDirectoryPair, LogDaily logDaily, LogRealTime logRealTime, string saveTaskName)
        {
            this.CurrentDirectoryPair = CurrentDirectoryPair;
            LogObserver = new List<Log.Log>();
            this.logDaily = logDaily;
            this.logRealTime = logRealTime;
            this.name = saveTaskName;
        }

        // Start the task
        internal abstract void Save();


        // Get the directory pair
        internal DirectoryPair GetDirectoryPair()
        {
            return CurrentDirectoryPair;
        }
    }
}
