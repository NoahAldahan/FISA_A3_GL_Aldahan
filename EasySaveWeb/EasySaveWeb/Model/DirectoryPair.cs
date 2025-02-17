using System.Text.Json.Serialization;

namespace EasySaveWeb.Model
{
    // Represents a pair of directories (source and target) used for save tasks.
    internal class DirectoryPair
    {
        // The source directory or file that needs to be backed up.
        [JsonInclude]
        internal string SourcePath { get; set; }

        // The target directory where the backup will be stored.
        // The TargetPath must be a directory, not a file.
        [JsonInclude]
        internal string TargetPath { get; set; }

        // Constructor: Initializes the directory pair with specified source and target paths.
        [JsonConstructor]
        internal DirectoryPair(string SourcePath, string TargetPath)
        {
            this.SourcePath = SourcePath;
            this.TargetPath = TargetPath;
        }

        // Copy Constructor: Creates a new DirectoryPair instance by copying an existing one.
        internal DirectoryPair(DirectoryPair dp)
        {
            this.SourcePath = dp.SourcePath;
            this.TargetPath = dp.TargetPath;
        }
    }
}

