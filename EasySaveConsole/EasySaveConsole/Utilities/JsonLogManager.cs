using EasySaveConsole.Model.Log;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

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
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating JSON file: {ex.Message}");
        }
        }

        // Add a backup to the daily file
        internal void AddSaveToDailyFile(DateTime Date, DailyInfo DailyInfo) { }

    internal void AddSaveToRealTimeFile(RealTimeInfo realTimeInfo)
    {
        List<object> saveList = new List<object>();

        // Charger le fichier JSON s'il existe
        if (File.Exists(LogRealTimePath))
        {
            try
            {
                string existingJson = File.ReadAllText(LogRealTimePath);

                // Désérialiser en liste d'objets
                saveList = JsonSerializer.Deserialize<List<object>>(existingJson) ?? new List<object>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement du JSON : {ex.Message}");
            }
        }
        else
        {
            //mettre un message pour le fihcier qui n'existe pas 
        }

        // Nouvelle sauvegarde à ajouter
        var jsonData = new
        {
            Name = realTimeInfo.Name,
            SourcePath = realTimeInfo.SourcePath,
            TargetPath = realTimeInfo.TargetPath,
            State = realTimeInfo.State,
            TotalFilesToCopy = realTimeInfo.TotalFilesToCopy,
            TotalFilesSize = realTimeInfo.TotalFilesSize,
            NbFilesLeftToDo = realTimeInfo.NbFilesLeftToDo,
            Progression = realTimeInfo.Progression,
            SaveDate = realTimeInfo.SaveDate
        };

        // Ajouter la nouvelle sauvegarde à la liste
        saveList.Add(jsonData);

        // Réécrire le fichier avec la nouvelle liste
        try
        {
            string updatedJson = JsonSerializer.Serialize(saveList, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(LogRealTimePath, updatedJson);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'écriture du JSON : {ex.Message}");
        }
    }


    // Retrieve the dates of the backup from the file
    internal DateTime GetAllDateDailyFile(string FilePath, DateTime Date) { return new DateTime(); }

        // Retrieve all backups from a path in the form (PATH - DATE)
        internal Dictionary<string, DateTime> GetAllSaveRealTimeFile(string FilePath) { throw new NotImplementedException(); }
    }
}
