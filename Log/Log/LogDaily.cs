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
            dailyInfo = new DailyInfo();
            stopWatch = new Stopwatch();
        }
        public void CreateDailyFile()
        {
            JsonLogManager.CreateDailyJsonFile(DateTime.Now, LogDailyPath);
        }

        public void AddDailyInfo(string SourcePath, string TargetPath)
        {
            dailyInfo.Name = "Name";
            dailyInfo.FileTransferTime = (stopWatch.ElapsedMilliseconds);
            dailyInfo.FileSource = SourcePath;
            dailyInfo.FileTarget = TargetPath;
            FileInfo fileInfo = new FileInfo(SourcePath);
            dailyInfo.FileSize = fileInfo.Length;
            dailyInfo.DateTime = DateTime.Now;
            JsonLogManager.AddSaveToDailyFile(dailyInfo, LogDailyPath);
            Console.WriteLine(dailyInfo.ToString());
        }
    }
}
