using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using EasySaveConsole.Model.Log;
using System.Runtime.CompilerServices;

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
            DailyInfo dailyInfo = new DailyInfo();
            FileAttributes sourceAttr = File.GetAttributes(CurrentDirectoryPair.SourcePath);
            FileAttributes targetAttr = File.GetAttributes(CurrentDirectoryPair.TargetPath);
            StopWatch.Reset();
            // if both paths are directories
            if (sourceAttr.HasFlag(FileAttributes.Directory) && targetAttr.HasFlag(FileAttributes.Directory))
            {
                DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(CurrentDirectoryPair.SourcePath);
                DirectoryInfo targetDirectoryInfo = new DirectoryInfo(CurrentDirectoryPair.TargetPath);
                StopWatch.Start();
                CopyFilesRecursivelyForTwoFolders(sourceDirectoryInfo, targetDirectoryInfo);

                StopWatch.Stop();
                dailyInfo.FileTransferTime = (StopWatch.ElapsedMilliseconds);
                dailyInfo.FileSource = CurrentDirectoryPair.SourcePath;
                dailyInfo.FileTarget = CurrentDirectoryPair.TargetPath;
                FileInfo fileInfo = new FileInfo(CurrentDirectoryPair.SourcePath);
                dailyInfo.FileSize = 112;
                dailyInfo.DateTime = DateTime.Now;
                SaveTaskInfo.Add("DailyInfo", dailyInfo);
                Notify();
            }
            // if the source path is a single file and the target a directory
            else if (!sourceAttr.HasFlag(FileAttributes.Directory) && targetAttr.HasFlag(FileAttributes.Directory))
            { 
                StopWatch.Start();
                File.Copy(CurrentDirectoryPair.SourcePath, Path.Combine(CurrentDirectoryPair.TargetPath, Path.GetFileName(CurrentDirectoryPair.SourcePath)), true);

                StopWatch.Stop();
                dailyInfo.FileTransferTime = (StopWatch.ElapsedMilliseconds);
                dailyInfo.FileSource = CurrentDirectoryPair.SourcePath;
                dailyInfo.FileTarget = CurrentDirectoryPair.TargetPath;
                FileInfo fileInfo = new FileInfo(CurrentDirectoryPair.SourcePath);
                dailyInfo.FileSize = fileInfo.Length;
                dailyInfo.DateTime = DateTime.Now;
                SaveTaskInfo.Add("DailyInfo", dailyInfo);
                Notify();
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
