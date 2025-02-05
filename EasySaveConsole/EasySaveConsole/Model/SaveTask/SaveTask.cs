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
        internal static string dailyInfoLibelle = "DailyInfo";
        internal static string RealTimeInfoLibelle = "RealTimeInfo";

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
        internal void UpdateLogs(string PathFileSource, string PathFileTarget)
        {
            Notify(new Dictionary<string, object>()
            {
                { dailyInfoLibelle, GetDailyInfo(PathFileSource, PathFileTarget) },
                { RealTimeInfoLibelle, GetRealTimeInfo(PathFileSource, PathFileTarget) }
            });
        }

        internal DailyInfo GetDailyInfo(string PathFileSource, string PathFileTarget)
        {
            DailyInfo dailyInfo = new DailyInfo();
            dailyInfo.Name = "Name";
            dailyInfo.FileTransferTime = (StopWatch.ElapsedMilliseconds);
            dailyInfo.FileSource = PathFileSource;
            dailyInfo.FileTarget = PathFileTarget;
            FileInfo fileInfo = new FileInfo(PathFileSource);
            dailyInfo.FileSize = fileInfo.Length;
            dailyInfo.DateTime = DateTime.Now;
            return dailyInfo;
        }

        internal RealTimeInfo GetRealTimeInfo(string PathFileSource, string PathFileTarget)
        {
            RealTimeInfo realTimeInfo = new RealTimeInfo();
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
        internal void Notify(Dictionary<string, object> SaveTaskInfo)
        {
            foreach (ILogObserver obs in LogObserver)
            {
                obs.Notify(SaveTaskInfo);
            }
        }
    }
}
