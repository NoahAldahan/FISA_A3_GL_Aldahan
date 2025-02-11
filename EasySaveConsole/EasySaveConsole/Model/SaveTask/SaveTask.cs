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
        internal List<Log.Log> LogObserver { get; set; }

        internal LogRealTime logRealTime;
        internal LogDaily logDaily;
 
         
        // Constructor
        [JsonConstructor]
        internal SaveTask(DirectoryPair CurrentDirectoryPair, LogDaily logDaily, LogRealTime logRealTime)
        {
            this.CurrentDirectoryPair = CurrentDirectoryPair;
            LogObserver = new List<Log.Log>();
            this.logDaily = logDaily;
            this.logRealTime = logRealTime;
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
