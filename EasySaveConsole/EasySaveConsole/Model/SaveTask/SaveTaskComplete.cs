using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using Log;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace EasySaveConsole.Model
{
    // Class representing a complete save task, inheriting from SaveTask.
    internal class SaveTaskComplete : SaveTask
    {
        // Constructor with a directory pair and task name.
        [JsonConstructor]
        internal SaveTaskComplete(DirectoryPair CurrentDirectoryPair, string name)
            : base(CurrentDirectoryPair, name) { }

        // Constructor with directory pair and log instances.
        internal SaveTaskComplete(DirectoryPair CurrentDirectoryPair, LogDaily logDaily, LogRealTime logRealTime, string saveTaskName)
            : base(CurrentDirectoryPair, logDaily, logRealTime, saveTaskName) { }

        // Overrides the abstract Save method to perform a complete backup.
        internal override bool Save()
        {
            // TODO: Improve error handling to provide users with a list of files that couldn't be saved.
            IsSaveSuccessful = true;
            try
            {
                SaveComplete(); // Perform the complete save process.
            }
            catch (Exception ex)
            {
                IsSaveSuccessful = false; // Mark the save as unsuccessful if an error occurs.
            }
            return IsSaveSuccessful;
        }

        // Performs the complete backup by copying files from source to target.
        private void SaveComplete()
        {
            logDaily.CreateDailyFile();
            logRealTime.CreateRealTimeInfo(name, CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath, ERealTimeState.ACTIVE, (int)ESaveTaskTypes.Complete);


            // Get file attributes to determine if the source and target are directories or files.
            FileAttributes sourceAttr = File.GetAttributes(CurrentDirectoryPair.SourcePath);
            FileAttributes targetAttr = File.GetAttributes(CurrentDirectoryPair.TargetPath);

            // Case 1: Both source and target are directories
            if (sourceAttr.HasFlag(FileAttributes.Directory) && targetAttr.HasFlag(FileAttributes.Directory))
            {
                DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(CurrentDirectoryPair.SourcePath);
                DirectoryInfo targetDirectoryInfo = new DirectoryInfo(CurrentDirectoryPair.TargetPath);
                CopyFilesRecursivelyForTwoFolders(sourceDirectoryInfo, targetDirectoryInfo);
            }
            // Case 2: Source is a file, target is a directory
            else if (!sourceAttr.HasFlag(FileAttributes.Directory) && targetAttr.HasFlag(FileAttributes.Directory))
            {
                logDaily.stopWatch.Restart(); // Start timing the copy operation.
                string FileName = Path.GetFileName(CurrentDirectoryPair.SourcePath);

                // Copy the file from source to target directory.
                File.Copy(CurrentDirectoryPair.SourcePath, Path.Combine(CurrentDirectoryPair.TargetPath, FileName), true);
                logDaily.stopWatch.Stop();
                //notify save of a new file
                logDaily.AddDailyInfo(name, CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath);
                logRealTime.UpdateRealTimeProgress();
            }
            // Case 3: The target path is not a directory (invalid case)
            else
                throw new Exception("The target path isn't a directory.");
        }

        // Recursively copies all files and subdirectories from the source to the target directory.
        private void CopyFilesRecursivelyForTwoFolders(DirectoryInfo sourceDirectoryInfo, DirectoryInfo targetDirectoryInfo)
        {
            // Iterate through all directories in the source and create them in the target.
            foreach (DirectoryInfo dir in sourceDirectoryInfo.GetDirectories())
            CopyFilesRecursivelyForTwoFolders(dir, targetDirectoryInfo.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in sourceDirectoryInfo.GetFiles())
            {
                logDaily.stopWatch.Restart(); // Start timing the file copy.

                // Copy file to the corresponding target directory.
                file.CopyTo(Path.Combine(targetDirectoryInfo.FullName, file.Name), true);
                logDaily.stopWatch.Stop();
                //notify save of a new file
                Console.WriteLine(file.FullName);
                logDaily.AddDailyInfo(name, file.FullName, targetDirectoryInfo.FullName + "\\" + file.Name);
                logRealTime.UpdateRealTimeProgress();
            }
        }

        internal override string GetStrSaveTaskType()
        {
            return "Complete";
        }
        internal override ESaveTaskTypes GetSaveTaskType()
        {
            return ESaveTaskTypes.Complete;
        }
    }
}

