using Log;
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
        internal SaveTaskDifferential(DirectoryPair CurrentDirectoryPair, LogDaily logDaily, LogRealTime logRealTime) : base(CurrentDirectoryPair, logDaily, logRealTime){}

        // Wrapper for the recursive function
        internal override bool Save()
        {
            // TODO : This way of checking isn't very clean, in future versions :
            // specify to the user every files that couldn't be saved
            IsSaveSuccessful = true;
            try
            {
                SaveDifferentialRecursive(CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath);
            }
            catch (Exception ex) {
                IsSaveSuccessful = false;
            }
            return IsSaveSuccessful;
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
                if (!File.Exists(targetFileInfo.FullName) || sourceFileInfo.LastWriteTime > JsonLogManager.GetLastSaveDate(this.logDaily.LogDailyPath, sourceFileInfo.FullName))
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
    }
}
