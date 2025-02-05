using System;
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
        internal Dictionary<string, object> SaveTaskInfo {  get; set; }  
        internal Stopwatch StopWatch { get; set; }
        internal List<ILogObserver> LogObserver { get; set; }

        internal void Notify() { 
            foreach(ILogObserver obs in LogObserver)
            {
                obs.Notify(SaveTaskInfo);
            }
        }

        // Constructor
        [JsonConstructor]
        internal SaveTask(DirectoryPair CurrentDirectoryPair) 
        {
            this.CurrentDirectoryPair = CurrentDirectoryPair;
            SaveTaskInfo = new Dictionary<string, object>();
            StopWatch = new Stopwatch();
            LogObserver = new List<ILogObserver>();
        }

        // Get the directory pair
        internal DirectoryPair GetDirectoryPair()
        {
            return CurrentDirectoryPair;
        }

        // Start the task
        internal abstract void Save();

        // Get the task information
        internal abstract List<string> GetInfo();

        internal void AddObserver(ILogObserver observer)
        {
            LogObserver.Add(observer);
        }
    }
}
