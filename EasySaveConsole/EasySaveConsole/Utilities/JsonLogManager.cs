using EasySaveConsole.Model.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasySaveConsole.Utilities
{
    internal class JsonLogManager
    {
        private string LogDailyPath;
        private string LogRealTimePath;

        internal JsonLogManager()
        {
            LogDailyPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..",
            Environment.GetEnvironmentVariable("LogPathDaily"));

            LogRealTimePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..",
            Environment.GetEnvironmentVariable("LogPathRealTime"));
        }

        // Create the daily backup file
        internal void CreateDailyJsonFile(DateTime Date) 
        {
            // Nom du fichier JSON basé sur la date
            string fileName = $"{LogDailyPath}\\backup_{Date:yyyy-MM-dd}.json";
            try { 
                if (!File.Exists(fileName))
                {
                    File.Create(fileName);
                }
                else
                {
                    Console.WriteLine($"Backup file already exists: {fileName}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating JSON file: {ex.Message}");
        }
        }

        // Add a backup to the daily file
        internal void AddSaveToDailyFile(DateTime Date, DailyInfo DailyInfo) { }

        // Retrieve the dates of the backup from the file
        internal DateTime GetAllDateDailyFile(string FilePath, DateTime Date) { return new DateTime(); }

        // Retrieve all backups from a path in the form (PATH - DATE)
        internal Dictionary<string, DateTime> GetAllSaveRealTimeFile(string FilePath) { throw new NotImplementedException(); }
    }
}
