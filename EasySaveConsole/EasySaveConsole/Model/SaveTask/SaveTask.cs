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

        internal abstract void SetRealTimeInfo(DirectoryPair PathParent, ERealTimeState state);

        // Get the directory pair
        internal DirectoryPair GetDirectoryPair()
        {
            return CurrentDirectoryPair;
        }

        internal abstract Tuple<int, int> GetTotalFilesInfosToCopy(string path);

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
            RealTimeInfo.Progression += ((1.0 / RealTimeInfo.TotalFilesToCopy) * 100);
            if (RealTimeInfo.NbFilesLeftToDo == 0)
            {
                RealTimeInfo.Progression = Convert.ToInt32(RealTimeInfo.Progression);
                RealTimeInfo.State = ERealTimeState.END.GetValue();
            }
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
