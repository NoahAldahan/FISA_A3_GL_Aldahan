using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Log
{
    public class JsonLogManager
    {
        private string LogDailyPath;
        private string LogRealTimePath;

        public JsonLogManager()
        {
        }
        public void SetLogDailyPath(string DailyPath)
        {
            LogDailyPath = DailyPath;
        }

        public void SetLogRealTimePath(string RealTimePath)
        {
            LogRealTimePath = RealTimePath;
        }
        public void UpdateRealTimeProgression(RealTimeInfo realTimeInfo)
        {
            List<RealTimeInfo> jsonObjectList = new List<RealTimeInfo>();
            if (File.Exists(LogRealTimePath))
            {
                try
                {
                    string json = File.ReadAllText(LogRealTimePath);
                    // Désérialiser en liste d'objets
                    jsonObjectList = JsonSerializer.Deserialize<List<RealTimeInfo>>(json) ?? new List<RealTimeInfo>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors du chargement du JSON : {ex.Message}");
                }
            }
            else
            {
                //mettre un message pour le fihcier existe pas  
            }
            int index = jsonObjectList.FindIndex(rt => rt.Name == realTimeInfo.Name);
            if (index != -1)
            {
                jsonObjectList[index] = realTimeInfo;
            }

            try
            {
                string updatedJson = JsonSerializer.Serialize(jsonObjectList, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(LogRealTimePath, updatedJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'écriture du JSON : {ex.Message}");
            }
        }

        public string GetFileDailyName(DateTime Date)
        {
            return $"{LogDailyPath}backup_{Date:yyyy-MM-dd}.json";
        }

        // Create the daily backup file
        public void CreateDailyJsonFile(DateTime Date)
        {
            // Nom du fichier JSON basé sur la date
            string fileName = GetFileDailyName(Date);
            try
            {
                if (!File.Exists(fileName))
                {
                    File.Create(fileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating JSON file: {ex.Message}");
            }
        }

        // Add a backup to the daily file
        public void AddSaveToDailyFile(DailyInfo DailyInfo)
        {
            string dailyInfoPath = GetFileDailyName(DailyInfo.DateTime);
            var jsonDailyInfo = new
            {
                DailyInfo.Name,
                DailyInfo.FileSource,
                DailyInfo.FileTarget,
                DailyInfo.FileSize,
                DailyInfo.FileTransferTime,
                DailyInfo.DateTime
            };
            AddJsonLogObject(dailyInfoPath, jsonDailyInfo);
        }

        public void AddSaveToRealTimeFile(RealTimeInfo realTimeInfo)
        {

            // Nouvelle sauvegarde à ajouter
            var jsonRealTimeInfo = new
            {
                realTimeInfo.Name,
                realTimeInfo.SourcePath,
                realTimeInfo.TargetPath,
                realTimeInfo.State,
                realTimeInfo.TotalFilesToCopy,
                realTimeInfo.TotalFilesSize,
                realTimeInfo.NbFilesLeftToDo,
                realTimeInfo.Progression,
                realTimeInfo.SaveDate
            };
            AddJsonLogObject(LogRealTimePath, jsonRealTimeInfo);
        }

        public void AddJsonLogObject(string FilePath, object LogObject)
        {
            List<object> jsonObjectList = new List<object>();
            if (File.Exists(FilePath))
            {
                try
                {
                    string json = File.ReadAllText(FilePath);
                    // Désérialiser en liste d'objets
                    if (string.IsNullOrEmpty(json))
                    {
                        json = "[]";
                    }
                    jsonObjectList = JsonSerializer.Deserialize<List<object>>(json) ?? new List<object>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors du chargement du JSON : {ex.Message}");
                }
            }
            else
            {
                //mettre un message pour le fihcier existe pas  
            }
            jsonObjectList.Add(LogObject);

            try
            {
                string updatedJson = JsonSerializer.Serialize(jsonObjectList, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(FilePath, updatedJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'écriture du JSON : {ex.Message}");
            }

        }

        // Retrieve the dates of the backup from the file
        public DateTime GetAllDateDailyFile(string FilePath, DateTime Date) { return new DateTime(); }

        // Retrieve all backups from a path in the form (PATH - DATE)
        internal Dictionary<string, DateTime> GetAllSaveRealTimeFile(string FilePath) { throw new NotImplementedException(); }

        public DateTime GetLastSaveDate(string FilePath)
        {
            DirectoryInfo d = new DirectoryInfo(FilePath);
            try
            {
                foreach (var file in d.GetFiles("*.json").OrderByDescending(f => f.CreationTime))
                {
                    string jsonContent = File.ReadAllText(file.Name);
                    List<DailyInfo> entities = JsonSerializer.Deserialize<List<DailyInfo>>(jsonContent);
                    DailyInfo foundEntity = entities.Find(e => e.FileSource == FilePath);
                    if (foundEntity.DateTime != null)
                    {
                        return foundEntity.DateTime;
                    }
                    else
                    {
                        continue;
                    }
                }
                return DateTime.MinValue;
                
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Erreur lors de la recherche de dernière sauvegarde. {ex}");
                return DateTime.MinValue;
            }
        }
    }

}
