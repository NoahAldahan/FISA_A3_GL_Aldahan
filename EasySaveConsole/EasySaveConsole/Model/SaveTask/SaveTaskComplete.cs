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
        internal SaveTaskComplete(DirectoryPair CurrentDirectoryPair, LogDaily logDaily, LogRealTime logRealTime) : base(CurrentDirectoryPair, logDaily, logRealTime){}

        // Start a complete save task
        internal override void Save()
        {
            logRealTime.CreateRealTimeInfo(CurrentDirectoryPair, ERealTimeState.ACTIVE);
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
                logDaily.AddDailyInfo(CurrentDirectoryPair);
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
                logDaily.AddDailyInfo(new DirectoryPair(file.FullName, targetDirectoryInfo.FullName + "\\" + file.Name));
                logRealTime.UpdateRealTimeProgress();
            }
        }

        // !!!! Méthode à modifier pour le incrémentiel (vérification des files à copier effectivement et en faire une liste)

    }
}
