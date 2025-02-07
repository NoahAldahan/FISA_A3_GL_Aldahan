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
        internal List<LogObserver> LogObserver { get; set; }

        internal DailyInfo DailyInfo;
        internal RealTimeInfo RealTimeInfo;



        // Constructor
        [JsonConstructor]
        internal SaveTask(DirectoryPair CurrentDirectoryPair)
        {
            this.CurrentDirectoryPair = CurrentDirectoryPair;
            LogObserver = new List<LogObserver>();
            StopWatch = new Stopwatch();
            RealTimeInfo = new RealTimeInfo();
        }

        // Start the task
        internal abstract void Save();

        internal abstract void SetRealTimeInfo(DirectoryPair PathParent);

        // Get the directory pair
        internal DirectoryPair GetDirectoryPair()
        {
            return CurrentDirectoryPair;
        }

        internal abstract Tuple<int, int> GetTotalFilesToCopy(string path);

        internal void SetDailyInfo(DirectoryPair PathFile)
        {
            DailyInfo.Name = "Name";
            DailyInfo.FileTransferTime = (StopWatch.ElapsedMilliseconds);
            DailyInfo.FileSource = PathFile.SourcePath;
            DailyInfo.FileTarget = PathFile.TargetPath;
            FileInfo fileInfo = new FileInfo(PathFile.SourcePath);
            DailyInfo.FileSize = fileInfo.Length;
            DailyInfo.DateTime = DateTime.Now;
        }

        internal void UpdateRealTimeProgress()
        {
            RealTimeInfo.NbFilesLeftToDo -= 1;
            RealTimeInfo.Progression += (int)((1.0 / RealTimeInfo.TotalFilesToCopy) * 100);
        }

        internal void AddObserver(LogObserver observer)
        {
            LogObserver.Add(observer);
        }
        // Set the task daily information
        internal void NotifyLogsCreate()
        {
            foreach (LogObserver observer in LogObserver)
            {
                observer.CreateNotify(RealTimeInfo);
            }
        }

        // Set the task daily information
        internal void NotifyLogsUpdate()
        {
            UpdateRealTimeProgress();
            foreach (LogObserver observer in LogObserver) 
            {
                observer.UdpateNotify(DailyInfo, RealTimeInfo);
            }
        }
    }
}
