﻿using System;
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
        internal SaveTaskComplete(DirectoryPair CurrentDirectoryPair) : base(CurrentDirectoryPair){}

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
                StopWatch.Restart();
                string FileName = Path.GetFileName(CurrentDirectoryPair.SourcePath);
                File.Copy(CurrentDirectoryPair.SourcePath, Path.Combine(CurrentDirectoryPair.TargetPath, FileName), true);
                StopWatch.Stop();
                
                NotifyLogsUpdate(new DirectoryPair(CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath + FileName));
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
                StopWatch.Restart();
                file.CopyTo(Path.Combine(targetDirectoryInfo.FullName, file.Name), true);
                StopWatch.Stop();
                NotifyLogsUpdate(new DirectoryPair(file.FullName, targetDirectoryInfo.FullName + file.Name));
            }
        }
    }
}
