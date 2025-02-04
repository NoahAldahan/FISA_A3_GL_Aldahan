using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace EasySaveConsole.Model
{
    internal class SaveTaskComplete : SaveTask
    {
        // Constructor
        internal SaveTaskComplete(DirectoryPair CurrentDirectoryPair) : base(CurrentDirectoryPair)
        {
        }

        // Start a complete save task
        internal override void Save()
        {
            FileAttributes sourceAttr = File.GetAttributes(CurrentDirectoryPair.SourcePath);
            FileAttributes targetAttr = File.GetAttributes(CurrentDirectoryPair.TargetPath);
            // if both paths are directories
            if (sourceAttr.HasFlag(FileAttributes.Directory) && targetAttr.HasFlag(FileAttributes.Directory))
            {
                DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(CurrentDirectoryPair.SourcePath);
                DirectoryInfo targetDirectoryInfo = new DirectoryInfo(CurrentDirectoryPair.TargetPath);
                CopyFilesRecursivelyForTwoFolders(sourceDirectoryInfo, targetDirectoryInfo);
            }
            // if the source path is a single file and the target a directory
            else if (!sourceAttr.HasFlag(FileAttributes.Directory) && targetAttr.HasFlag(FileAttributes.Directory))
            {
                File.Copy(CurrentDirectoryPair.SourcePath, Path.Combine(CurrentDirectoryPair.TargetPath, Path.GetFileName(CurrentDirectoryPair.SourcePath)), true);
            }
            // if the target isn't a directory
            else
                throw new Exception("The target path isn't a directory.");
        }

        // Copies files recursively from a source directory to a target directory
        private void CopyFilesRecursivelyForTwoFolders(DirectoryInfo sourceDirectoryInfo, DirectoryInfo targetDirectoryInfo)
        {
            foreach (DirectoryInfo dir in sourceDirectoryInfo.GetDirectories())
                CopyFilesRecursivelyForTwoFolders(dir, targetDirectoryInfo.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in sourceDirectoryInfo.GetFiles())
                file.CopyTo(Path.Combine(targetDirectoryInfo.FullName, file.Name), true);
        }

        // Get the task information
        internal override List<string> GetInfo()
        {
            List<string> info = new List<string>();
            info.Add("Complete save task");
            info.Add("Source path: " + CurrentDirectoryPair.SourcePath);
            info.Add("Target path: " + CurrentDirectoryPair.TargetPath);
            return info;
        }
    }
}
