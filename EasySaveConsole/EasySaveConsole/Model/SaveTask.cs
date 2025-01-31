using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EasySaveConsole.Model
{
    [JsonDerivedType(typeof(SaveTaskComplete), "SaveTaskComplete")]
    [JsonDerivedType(typeof(SaveTaskDifferential), "SaveTaskDifferential")]
    public abstract class SaveTask
    {
        // The directory name storing the target and source directories
        [JsonInclude]
        public DirectoryPair CurrentDirectoryPair { get; set; }

        // Constructor
        [JsonConstructor]
        public SaveTask(DirectoryPair CurrentDirectoryPair) 
        {
            this.CurrentDirectoryPair = CurrentDirectoryPair;
        }

        // Get the directory pair
        public DirectoryPair GetDirectoryPair()
        {
            return CurrentDirectoryPair;
        }

        // Start the task
        public abstract void Save();

        // Get the task information
        public abstract List<string> GetInfo();
    }
}
