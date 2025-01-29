using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model
{
    public abstract class SaveTask
    {
        // The directory name storing the target and source directories
        protected DirectoryPair CurrentDirectoryPair { get; set; }
        public SaveTask(DirectoryPair directoryPair) 
        {
            CurrentDirectoryPair = directoryPair;
        }

        // Start the task
        public abstract void Save();

        // Get the task information
        public abstract string GetInfo();
    }
}
