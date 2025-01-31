using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EasySaveConsole.Model
{
    public class DirectoryPair
    {
        // The source directory or file
        public string SourcePath { get; set; }

        // The target directory, TargetPath needs to be a directory
        public string TargetPath { get; set; }

        //Constructor
        [JsonConstructor]
        public DirectoryPair(string SourcePath, string TargetPath)
        {
            this.SourcePath = SourcePath;
            this.TargetPath = TargetPath;
        }
    }
}
