using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EasySaveConsole.Model.Log;

namespace EasySaveConsole.Model
{
    [JsonDerivedType(typeof(SaveTaskComplete), "SaveTaskComplete")]
    [JsonDerivedType(typeof(SaveTaskDifferential), "SaveTaskDifferential")]
    internal abstract class SaveTask
    {
        // The directory name storing the target and source directories
        [JsonInclude]
        internal DirectoryPair CurrentDirectoryPair { get; set; }
        internal Stopwatch StopWatch { get; set; }
        internal List<ILogObserver> LogObserver { get; set; }

        // Constructor
        [JsonConstructor]
        internal SaveTask(DirectoryPair CurrentDirectoryPair)
        {
            this.CurrentDirectoryPair = CurrentDirectoryPair;
            LogObserver = new List<ILogObserver>();
            StopWatch = new Stopwatch();
        }

        // Start the task
        internal abstract void Save();

        // Set the task daily information
        internal void NotifyLogsCreate(DirectoryPair PathParent)
        {
            NotifyLog(GetRealTimeInfo(PathParent));
        }

        // Set the task daily information
        internal void NotifyLogsUpdate(DirectoryPair PathFile)
        {
            NotifyLog(GetDailyInfo(PathFile));
        }

        internal DailyInfo GetDailyInfo(DirectoryPair PathFile)
        {
            DailyInfo dailyInfo = new DailyInfo();
            dailyInfo.Name = "Name";
            dailyInfo.FileTransferTime = (StopWatch.ElapsedMilliseconds);
            dailyInfo.FileSource = PathFile.SourcePath;
            dailyInfo.FileTarget = PathFile.TargetPath;
            FileInfo fileInfo = new FileInfo(PathFile.SourcePath);
            dailyInfo.FileSize = fileInfo.Length;
            dailyInfo.DateTime = DateTime.Now;
            return dailyInfo;
        }

        internal RealTimeInfo GetRealTimeInfo(DirectoryPair PathParent)
        {
            RealTimeInfo realTimeInfo = new RealTimeInfo();
            realTimeInfo.Name = "Name";
            realTimeInfo.SourcePath = PathParent.SourcePath;
            realTimeInfo.TargetPath = PathParent.TargetPath;
            FileInfo fileInfo = new FileInfo(PathParent.SourcePath);
            realTimeInfo.TotalFilesToCopy = 0;
            realTimeInfo.NbFilesLeftToDo = 0;
            realTimeInfo.Progression = 0;
            return realTimeInfo;
        }

        // Get the directory pair
        internal DirectoryPair GetDirectoryPair()
        {
            return CurrentDirectoryPair;
        }

        internal void AddObserver(ILogObserver observer)
        {
            LogObserver.Add(observer);
        }

        internal void NotifyLog(object Infos)
        {
            foreach (ILogObserver obs in LogObserver)
            {
                if (Infos is RealTimeInfo realTimeInfo)
                {
                    obs.Notify(realTimeInfo);
                }
                else if (Infos is DailyInfo fileInfo) 
                {
                    obs.Notify(fileInfo);
                }
            }
        }
    }
}
