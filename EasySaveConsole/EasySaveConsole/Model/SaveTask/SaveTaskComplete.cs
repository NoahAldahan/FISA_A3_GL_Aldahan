using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Log;
using System.Runtime.CompilerServices;

namespace EasySaveConsole.Model
{
    internal class SaveTaskComplete : SaveTask
    {
        // Constructor
        internal SaveTaskComplete(DirectoryPair CurrentDirectoryPair, LogDaily logDaily, LogRealTime logRealTime, string saveTaskName) : base(CurrentDirectoryPair, logDaily, logRealTime, saveTaskName){}

        // Start a complete save task
        internal override bool Save()
        {
            // TODO : This way of checking isn't very clean, in future versions :
            // specify to the user every files that couldn't be saved
            IsSaveSuccessful = true;
            try
            {
                SaveComplete();
            }
            catch (Exception ex)
            {
                IsSaveSuccessful = false;
            }
            return IsSaveSuccessful;
        }

        private void SaveComplete()
        {
            logRealTime.CreateRealTimeInfo(CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath, ERealTimeState.ACTIVE);
            logDaily.CreateDailyFile();
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
                logDaily.stopWatch.Restart();
                string FileName = Path.GetFileName(CurrentDirectoryPair.SourcePath);
                File.Copy(CurrentDirectoryPair.SourcePath, Path.Combine(CurrentDirectoryPair.TargetPath, FileName), true);
                logDaily.stopWatch.Stop();
                //notify save of a new file
                logDaily.AddDailyInfo(CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath);
                logRealTime.UpdateRealTimeProgress();
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
            {
                logDaily.stopWatch.Restart();
                file.CopyTo(Path.Combine(targetDirectoryInfo.FullName, file.Name), true);
                logDaily.stopWatch.Stop();
                //notify save of a new file
                logDaily.AddDailyInfo(file.FullName, targetDirectoryInfo.FullName + "\\" + file.Name);
                logRealTime.UpdateRealTimeProgress();
            }
        }
    }
}
