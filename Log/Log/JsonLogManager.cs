using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;

namespace Log
{
    public class JsonLogManager
    {
        internal static void UpdateRealTimeProgression(RealTimeInfo realTimeInfo, string LogRealTimePath)
        {
            List<RealTimeInfo> jsonObjectList = new List<RealTimeInfo>();
            string fileName = GetFileRealTimeName(LogRealTimePath);
            if (File.Exists(fileName))
            {
                try
                {
                    string json = AsyncFileManager.LockedReadAllText(fileName);
                    // Désérialiser en liste d'objets
                    jsonObjectList = AsyncFileManager.LockedDeserialize<List<RealTimeInfo>>(json) ?? new List<RealTimeInfo>();
                }
                catch (Exception ex)
                {
                    throw new Exception("Log JSON UpdateRealTimeProgression, readalltext and deserialize");
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
            else if (index == -1)
            {
                jsonObjectList.Add(realTimeInfo);
            }

            try
            {
                if (jsonObjectList.Count > 0) Trace.WriteLine(Thread.CurrentThread.ManagedThreadId +  "  Start locked serialize" + jsonObjectList[0].ToString());
                else Trace.WriteLine(Thread.CurrentThread.ManagedThreadId + "  Start locked serialize jsonobecject list empty");
                string updatedJson = AsyncFileManager.LockedSerialize(jsonObjectList);
                Trace.WriteLine(Thread.CurrentThread.ManagedThreadId + "  Start locked write all text");
                AsyncFileManager.LockedWriteAllText(fileName, updatedJson);
                Trace.WriteLine(Thread.CurrentThread.ManagedThreadId + "  Start locked write all text");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(Thread.CurrentThread.ManagedThreadId + "  " + ex.Message);
                throw new Exception("Log JSON UpdateRealTimeProgression, serialize and write all text");
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
                throw new Exception("Log JSON CreateDailyJsonFile");
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
                throw new Exception("Log JSON CreateRealTimeJsonFile");
            }
        }

        public static DateTime GetLastSaveDateFromJson(string LogDailyPath, string FilePath)
        {
            DirectoryInfo LogDailyDirectory = new DirectoryInfo(LogDailyPath);
            try
            {
                foreach (var file in LogDailyDirectory.GetFiles("*.json").OrderByDescending(f => f.CreationTime))
                {
                    string jsonContent = AsyncFileManager.LockedReadAllText(file.FullName);
                    List<DailyInfo> entities = AsyncFileManager.LockedDeserialize<List<DailyInfo>>(jsonContent);
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
            try
            {
                string json = "";
                if (File.Exists(FilePath))
                {
                    json = AsyncFileManager.LockedReadAllText(FilePath);
                }
                    
                // Désérialiser en liste d'objets
                if (string.IsNullOrEmpty(json))
                {
                    json = "[]";
                }

                List<RealTimeInfo> jsonObjectList = AsyncFileManager.LockedDeserialize<List<RealTimeInfo>>(json) ?? new List<RealTimeInfo>();
                // Remove any existing object with the same Name
                jsonObjectList.RemoveAll(rt => rt.Name == realTimeInfo.Name);
                jsonObjectList.Add(realTimeInfo);
                string updatedJson = AsyncFileManager.LockedSerialize(jsonObjectList);
                AsyncFileManager.LockedWriteAllText(FilePath, updatedJson);
            }
            catch (Exception ex)
            {
                throw new Exception("Log JSON AddJsonLogObjectRealTime");
            }
        }

        internal static void AddJsonLogObjectDailyInfo(string FilePath, DailyInfo dailyInfo)
        {
            FilePath = GetFileDailyName(DateTime.Now, FilePath);
            try 
            {
                string json = "";
                if (File.Exists(FilePath))
                    json = AsyncFileManager.LockedReadAllText(FilePath);

                // Désérialiser en liste d'objets
                if (string.IsNullOrEmpty(json))
                {
                    json = "[]";
                }
                List<DailyInfo> jsonObjectList = AsyncFileManager.LockedDeserialize<List<DailyInfo>>(json) ?? new List<DailyInfo>();
                // Remove any existing object with the same Name
                jsonObjectList.Add(dailyInfo);
                
                string updatedJson = AsyncFileManager.LockedSerialize(jsonObjectList);
                AsyncFileManager.LockedWriteAllText(FilePath, updatedJson);
            }
            catch (Exception ex)
            {
                throw new Exception("Log JSON AddJsonLogObjectDailyInfo");
            }
        }
    }
}
