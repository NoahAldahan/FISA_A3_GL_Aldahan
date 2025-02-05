using System.Text.Json.Serialization;

namespace EasySaveConsole.Model
{
    internal class DirectoryPair
    {
        // The source directory or file
        internal string SourcePath { get; set; }

        // The target directory, TargetPath needs to be a directory
        internal string TargetPath { get; set; }

        //Constructor
        [JsonConstructor]
        internal DirectoryPair(string SourcePath, string TargetPath)
        {
            this.SourcePath = SourcePath;
            this.TargetPath = TargetPath;
        }
        internal DirectoryPair(DirectoryPair dp)
        {
            this.SourcePath = dp.SourcePath;
            this.TargetPath = dp.TargetPath;
        }
    }
}
