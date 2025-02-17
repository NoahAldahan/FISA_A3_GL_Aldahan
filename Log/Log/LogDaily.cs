using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    public class LogDaily : Log
    {
        public DailyInfo dailyInfo;
        public Stopwatch stopWatch { get; set; }

        public LogDaily(string LogDailyPath, string LogRealTimePath) : base(LogDailyPath, LogRealTimePath)
        {
            stopWatch = new Stopwatch();
        }
        public void CreateDailyFile()
        {
            JsonLogManager.CreateRepertories(LogDailyPath);
            JsonLogManager.CreateDailyJsonFile(DateTime.Now, LogDailyPath);
        }


        public void AddDailyInfo(string saveTaskName, string SourcePath, string TargetPath)
        {
            dailyInfo.Name = saveTaskName;
            dailyInfo.FileTransferTime = (stopWatch.ElapsedMilliseconds);
            dailyInfo.FileSource = SourcePath;
            dailyInfo.FileTarget = TargetPath; 
            FileInfo fileInfo = new FileInfo(SourcePath);
            dailyInfo.FileSize = fileInfo.Length;
            dailyInfo.DateTime = DateTime.Now;
            JsonLogManager.AddSaveToDailyFile(dailyInfo, LogDailyPath);
        }
    }
}
