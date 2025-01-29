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
        public SaveTaskComplete(DirectoryPair directoryPair) : base(directoryPair)
        {
        }

        public override void Save()
        {
            FileAttributes sourceAttr = File.GetAttributes(CurrentDirectoryPair.SourcePath);
            FileAttributes targetAttr = File.GetAttributes(CurrentDirectoryPair.TargetPath);
            // if both paths are directories
            if (sourceAttr.HasFlag(FileAttributes.Directory) && targetAttr.HasFlag(FileAttributes.Directory))
                SaveComplete(CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath);
            // if the source path is a single file and the target a directory
            else if (!sourceAttr.HasFlag(FileAttributes.Directory) && targetAttr.HasFlag(FileAttributes.Directory))
            {
                File.Copy(CurrentDirectoryPair.SourcePath, Path.Combine(CurrentDirectoryPair.TargetPath, Path.GetFileName(CurrentDirectoryPair.SourcePath)), true);
            }
            // if the target isn't a directory
            else
                throw new Exception("The target path isn't a directory.");
        }
        public void SaveComplete(string sourcePathString, string targetPathString)
        {
            DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(sourcePathString);
            DirectoryInfo targetDirectoryInfo = new DirectoryInfo(targetPathString);
            CopyFilesRecursivelyForTwoFolders(sourceDirectoryInfo, targetDirectoryInfo);
        }
        private void CopyFilesRecursivelyForTwoFolders(DirectoryInfo sourceDirectoryInfo, DirectoryInfo targetDirectoryInfo)
        {
            foreach (DirectoryInfo dir in sourceDirectoryInfo.GetDirectories())
                CopyFilesRecursivelyForTwoFolders(dir, targetDirectoryInfo.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in sourceDirectoryInfo.GetFiles())
                file.CopyTo(Path.Combine(targetDirectoryInfo.FullName, file.Name), true);
        }
        public override string GetInfo()
        {
            throw new System.NotImplementedException();
        }
    }
}
