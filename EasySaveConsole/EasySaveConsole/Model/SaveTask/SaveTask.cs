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
    internal abstract class SaveTask
    {
        // The directory name storing the target and source directories
        [JsonInclude]
        internal DirectoryPair CurrentDirectoryPair { get; set; }

        // Constructor
        [JsonConstructor]
        internal SaveTask(DirectoryPair CurrentDirectoryPair) 
        {
            this.CurrentDirectoryPair = CurrentDirectoryPair;
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
    }
}
