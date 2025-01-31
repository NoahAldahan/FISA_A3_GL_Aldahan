using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model
{
    internal class SaveTaskDifferential : SaveTask
    {
        // Constructor
        public SaveTaskDifferential(DirectoryPair CurrentDirectoryPair) : base(CurrentDirectoryPair)
        {
        }

        // Wrapper for the recursive function
        public override void Save()
        {
            SaveDifferentialRecursive(CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath);
        }

        // Recursive function to save the updated files and directories since the last save
        private void SaveDifferentialRecursive(string SourcePath, string TargetPath)
        {
            FileAttributes sourceAttr = File.GetAttributes(SourcePath);

            // If it is a file
            if (!sourceAttr.HasFlag(FileAttributes.Directory))
            {
                FileInfo sourceFileInfo = new FileInfo(SourcePath);
                FileInfo targetFileInfo = new FileInfo(Path.Combine(TargetPath, sourceFileInfo.Name));
                // If the file doesn't exist or the source file is more recent than the target file
                // We use targetFileInfo.FullName instead of TargetPath because we need the full path of the file
                // (with the name of the file appended) that is going to be created or updated
                // TEMP : replace targetFileInfo.LastWriteTime with value saved from logs
                if (!File.Exists(targetFileInfo.FullName) || sourceFileInfo.LastWriteTime > targetFileInfo.LastWriteTime)
                {
                    File.Copy(SourcePath, targetFileInfo.FullName, true);
                }
            }
            // If it is a directory
            else
            {
                DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(SourcePath);
                DirectoryInfo targetDirectoryInfo = new DirectoryInfo(TargetPath);
                // If the directory is empty and the target directory doesn't exist
                if (sourceDirectoryInfo.GetDirectories().Length == 0 && !targetDirectoryInfo.Exists)
                {
                    targetDirectoryInfo.Create();
                }
                else
                {
                    // Save every file and directory in the source directory
                    foreach (DirectoryInfo dir in sourceDirectoryInfo.GetDirectories())
                    {
                        SaveDifferentialRecursive(dir.FullName, Path.Combine(targetDirectoryInfo.FullName, dir.Name));
                    }
                    foreach(FileInfo file in sourceDirectoryInfo.GetFiles())
                    {
                        SaveDifferentialRecursive(file.FullName, targetDirectoryInfo.FullName);
                    }
                }
            }
        }

        // Get the task information
        public override List<string> GetInfo() 
        {
            List<string> infos = new List<string>();
            infos.Add("Differential save task");
            infos.Add("Source path : " + CurrentDirectoryPair.SourcePath);
            infos.Add("Target path : " + CurrentDirectoryPair.TargetPath);
            return infos;
        }
    }
}
