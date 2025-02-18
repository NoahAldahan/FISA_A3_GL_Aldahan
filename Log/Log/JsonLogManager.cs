using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Log
{
    internal class JsonLogManager
    {
        internal static void UpdateRealTimeProgression(RealTimeInfo realTimeInfo, string LogRealTimePath)
        {
            List<RealTimeInfo> jsonObjectList = new List<RealTimeInfo>();
            string fileName = GetFileRealTimeName(LogRealTimePath);
            if (File.Exists(fileName))
            {
                try
                {
                    string json = File.ReadAllText(fileName);
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
                File.WriteAllText(fileName, updatedJson);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'écriture du JSON : {ex.Message}");
            }
        }

        // Create a desired repertory if it does not exist
        internal static void CreateRepertories(string path)
        {

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

        }
        internal static string GetFileDailyName(DateTime Date, string LogDailyPath)
        { 
            return $"{LogDailyPath}backup_{Date:yyyy-MM-dd}.json"; 
        }

        internal static string GetFileRealTimeName(string  LogRealTimePath)
        {
            return $"{LogRealTimePath}RealTimeSave.json";
        }

        // Create the daily backup file
        internal static void CreateDailyJsonFile(DateTime Date, string LogDailyPath)
        {
            // Nom du fichier JSON basé sur la date
            try
            {
                if (!File.Exists(LogDailyPath))
                {
                    File.Create(LogDailyPath);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating JSON file: {ex.Message}");
            }
        }

        // Create the real time json log file
        internal static void CreateRealTimeJsonFile(string LogRealTimePath)
        {
            // Nom du fichier JSON basé sur la date
            string fileName = GetFileRealTimeName(LogRealTimePath);
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

        internal static DateTime GetLastSaveDateFromJson(string LogDailyPath, string FilePath)
        {
            DirectoryInfo LogDailyDirectory = new DirectoryInfo(LogDailyPath);
            try
            {
                foreach (var file in LogDailyDirectory.GetFiles("*.json").OrderByDescending(f => f.CreationTime))
                {
                    string jsonContent = File.ReadAllText(file.FullName);
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

        internal static void AddJsonLogObjectRealTime(string FilePath, RealTimeInfo realTimeInfo)
        {
            FilePath = GetFileRealTimeName(FilePath);
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
                    List<RealTimeInfo> jsonObjectList = JsonSerializer.Deserialize<List<RealTimeInfo>>(json) ?? new List<RealTimeInfo>();
                    // Remove any existing object with the same Name
                    jsonObjectList.RemoveAll(rt => rt.Name == realTimeInfo.Name);
                    jsonObjectList.Add(realTimeInfo);
                    string updatedJson = JsonSerializer.Serialize(jsonObjectList, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(FilePath, updatedJson);
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
        }

        internal static void AddJsonLogObjectDailyInfo(string FilePath, DailyInfo dailyInfo)
        {
            FilePath = GetFileDailyName(DateTime.Now, FilePath);
            CreateDailyJsonFile(DateTime.Now, FilePath);
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
                    List<DailyInfo> jsonObjectList = JsonSerializer.Deserialize<List<DailyInfo>>(json) ?? new List<DailyInfo>();
                    // Remove any existing object with the same Name
                    jsonObjectList.Add(dailyInfo);
                    string updatedJson = JsonSerializer.Serialize(jsonObjectList, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(FilePath, updatedJson);
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
        }
    }

}
