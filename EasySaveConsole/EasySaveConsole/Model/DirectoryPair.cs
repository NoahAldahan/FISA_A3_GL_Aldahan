using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model
{
    public class DirectoryPair
    {
        public string SourcePath { get; set; }

        // TargetPath needs to be a directory
        public string TargetPath { get; set; }

        public DirectoryPair(string SourcePath, string TargetPath)
        {
            this.SourcePath = SourcePath;
            this.TargetPath = TargetPath;
        }
    }
}
