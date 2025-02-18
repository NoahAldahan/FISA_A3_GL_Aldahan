using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using Log;
using System.Threading.Tasks;
using EasySaveWPFApp.Utilities;

namespace EasySaveWPFApp.Model
{
    // Specifies that the class can be serialized as a derived type in JSON format.
    [JsonDerivedType(typeof(SaveTaskComplete), "SaveTaskComplete")]
    [JsonDerivedType(typeof(SaveTaskDifferential), "SaveTaskDifferential")]
    internal abstract class SaveTask
    {
        // Stores the source and target directory pair for the backup task.
        [JsonInclude]
        internal DirectoryPair CurrentDirectoryPair { get; set; }

        // Boolean flag to track whether the save operation was successful.
        protected bool IsSaveSuccessful;

        // Logs for real-time and daily backup operations.
        internal LogRealTime logRealTime;
        internal LogDaily logDaily;
        //
        internal List<string> UnsavedPaths;

        // Name of the backup task.
        [JsonInclude]
        internal string name;

        // Setter for the real-time logging instance.
        internal void SetLogRealTime(LogRealTime logRealTime)
        {
            this.logRealTime = logRealTime;
        }

        // Setter for the daily logging instance.
        internal void SetLogDaily(LogDaily logDaily)
        {
            this.logDaily = logDaily;
        }

        // Setter for the daily logging instance.
        internal List<string> GetUnsavedPaths()
        {
            return new List<string>(UnsavedPaths);
        }

        // Constructor for the SaveTask class with a directory pair and task name.
        [JsonConstructor]
        internal SaveTask(DirectoryPair CurrentDirectoryPair, string name)
        {
            this.CurrentDirectoryPair = CurrentDirectoryPair;
            this.name = name;
            this.UnsavedPaths = new List<string>();
        }

        // Overloaded constructor with additional parameters for logging instances.
        internal SaveTask(DirectoryPair CurrentDirectoryPair, LogDaily logDaily, LogRealTime logRealTime, string saveTaskName)
        {
            this.CurrentDirectoryPair = CurrentDirectoryPair;
            this.logDaily = logDaily;
            this.logRealTime = logRealTime;
            this.name = saveTaskName;
            this.UnsavedPaths = new List<string>();
        }

        // Returns the directory pair associated with the backup task.
        internal DirectoryPair GetDirectoryPair()
        {
            return CurrentDirectoryPair;
        }

        internal abstract EMessage GetMessageSaveTaskType();
        internal abstract ESaveTaskTypes GetSaveTaskType();

        // Start the task
        // Returns true if the task was successful (all files were saved), false otherwise
        // To get the paths of all the files and directories unsaved, call GetUnsavedPaths().
        internal abstract bool Save(List<string> EncryptingExtensions);

        // Method that will be called when a single file is copied (called by recursive and non-recursives)
        // It also handles log calls when a file is copied
        // Can throw unauthorized access exception
        internal void CopySingleFile(string sourcePath, string targetPath, List<string> EncryptingExtensions)
        {
            logDaily.stopWatch.Restart(); // Démarrer le chrono pour la copie
            File.Copy(sourcePath, targetPath, true); // Copier le fichier
            logDaily.stopWatch.Stop(); // Arrêter le chrono après la copie

            long encryptionTime = 0; // Par défaut, pas de cryptage

            // Vérifier si le fichier doit être crypté
            if (EncryptingExtensions.Contains(Path.GetExtension(targetPath)))
            {
                try
                {
                    Stopwatch encryptionStopwatch = Stopwatch.StartNew(); // Démarrer le chrono pour le cryptage
                    CryptoSoftLibrary.CryptoSoftLibrary.EncryptFile(targetPath, JsonManager.EncryptionKey);
                    encryptionStopwatch.Stop(); // Arrêter le chrono après le cryptage
                    encryptionTime = encryptionStopwatch.ElapsedMilliseconds; // Temps de cryptage en ms
                }
                catch (Exception ex)
                {
                    encryptionTime = -1; // Code erreur par défaut
                    Console.WriteLine($"Erreur lors du cryptage du fichier {targetPath} : {ex.Message}");
                }
            }

            // Enregistrer dans le log avec le temps de cryptage
            logDaily.AddDailyInfo(name, CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath, encryptionTime);
            logRealTime.UpdateRealTimeProgress();
        }

    }
}

