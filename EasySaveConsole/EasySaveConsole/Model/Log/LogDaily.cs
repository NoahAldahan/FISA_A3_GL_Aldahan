using EasySaveConsole.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model.Log
{
    internal class LogDaily : Log
    {
        DailyInfo dailyInfo;


        internal Stopwatch stopWatch { get; set; }

        internal LogDaily(JsonLogManager jsonLogManager) : base(jsonLogManager){
            dailyInfo = new DailyInfo();
            stopWatch = new Stopwatch();
        }
        internal void CreateDailyFile()
        {
            jsonLogManager.CreateDailyJsonFile(DateTime.Now);
        }

        internal void AddDailyInfo(DirectoryPair PathFile)
        {
            dailyInfo.Name = "Name";
            dailyInfo.FileTransferTime = (stopWatch.ElapsedMilliseconds);
            dailyInfo.FileSource = PathFile.SourcePath;
            dailyInfo.FileTarget = PathFile.TargetPath;
            FileInfo fileInfo = new FileInfo(PathFile.SourcePath);
            dailyInfo.FileSize = fileInfo.Length;
            dailyInfo.DateTime = DateTime.Now;
            jsonLogManager.AddSaveToDailyFile(dailyInfo);
            Console.WriteLine(dailyInfo.ToString());
        }
    }
}
