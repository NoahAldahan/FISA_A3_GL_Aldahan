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
