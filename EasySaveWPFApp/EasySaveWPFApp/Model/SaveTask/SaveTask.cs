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
using System.ComponentModel;
using EasySaveWPFApp.Utilities;
using System.Threading;
using DotNetEnv;
using System.Diagnostics.Eventing.Reader;

namespace EasySaveWPFApp.Model
{
    // Specifies that the class can be serialized as a derived type in JSON format.
    [JsonDerivedType(typeof(SaveTaskComplete), "SaveTaskComplete")]
    [JsonDerivedType(typeof(SaveTaskDifferential), "SaveTaskDifferential")]
    public abstract class SaveTask : INotifyPropertyChanged
    {
        protected CancellationTokenSource cancellationTokenSource;
        protected ManualResetEventSlim pauseEvent;

        // Stores the source and target directory pair for the backup task.
        [JsonInclude]
        internal DirectoryPair CurrentDirectoryPair { get; set; }

        // Boolean flag to track whether the save operation was successful.
        protected bool IsSaveSuccessful;

        // Boolean flag to track whether the save operation was successful.
        internal float SaveTaskProgressPercentage;

        // Logs for real-time and daily backup operations.
        internal LogRealTime logRealTime;
        internal LogDaily logDaily;
        // List of unsaved paths in case of unauthorized accesses. Will not be filled if the save task is canceled
        internal List<string> UnsavedPaths;
        // Number of files left for save when cancelled
        internal int nFilesUnsavedCancelled;
        // Boolean to check if the priority save loop is over
        bool IsPriorityLoopOver;
        // Boolean to check if the non priority save loop is over
        bool IsNonPriorityLoopOver;
        // Name of the backup task.
        [JsonInclude]
        internal string name;
        internal ERealTimeState state;

        public string BindName
        {
            get => name;
            set { name = value; OnPropertyChanged(nameof(BindName)); }
        }
        public string BindState
        {
            get
            {
                switch (state)
                {
                    case ERealTimeState.ACTIVE:
                        return Resources.Resources.SaveTask_StateActive;
                    case ERealTimeState.END:
                        return Resources.Resources.SaveTask_StateEnd;
                    case ERealTimeState.ERROR:
                        return Resources.Resources.SaveTask_StateError;
                    case ERealTimeState.PAUSED:
                        return Resources.Resources.SaveTask_StatePaused;
                    case ERealTimeState.STOPPED:
                        return Resources.Resources.SaveTask_StateStopped;
                    case ERealTimeState.WAITING_FOR_PRIORITY_FILES:
                        return Resources.Resources.SaveTask_StateWaitingForPriorityFiles;
                    default:
                        return Resources.Resources.SaveTask_StateINVALID;
                }
            }
        }
        public void SetBindState(ERealTimeState state) { this.state = state; OnPropertyChanged(nameof(BindState)); 
            this.SaveTaskProgressStateChangedCallback?.Invoke(BindState.ToString()); }
        [JsonIgnore]
        public Action<string> SaveTaskProgressPercentageChangedCallback { get; set; }

        [JsonIgnore]
        public Action<string> SaveTaskProgressStateChangedCallback { get; set; }

        public float BindSaveTaskProgressPercentage
        {
            get => SaveTaskProgressPercentage;
            set {
                Trace.WriteLine($"Propriété modifiée progress");
                if (value > 100.0f) SaveTaskProgressPercentage = 100.0f;
                else if (value < 0.0f) SaveTaskProgressPercentage = 0.0f; 
                else if (value == float.NegativeInfinity) SaveTaskProgressPercentage = 0.0f;
                else if (value == float.PositiveInfinity) SaveTaskProgressPercentage = 100.0f;
                else SaveTaskProgressPercentage = value;
                Trace.WriteLine(SaveTaskProgressPercentage);
                OnPropertyChanged(nameof(BindSaveTaskProgressPercentage)); 
                this.SaveTaskProgressPercentageChangedCallback?.Invoke(SaveTaskProgressPercentage.ToString());
            }
        }

        public string BindSource
        {
            get => CurrentDirectoryPair.SourcePath;
            set { CurrentDirectoryPair.SourcePath = value; OnPropertyChanged(nameof(BindSource)); }
        }

        public string BindDestination
        {
            get => CurrentDirectoryPair.TargetPath;
            set { CurrentDirectoryPair.TargetPath = value; OnPropertyChanged(nameof(BindDestination)); }
        }

        public ESaveTaskTypes BindSaveTaskType
        {
            get => GetSaveTaskType();
            set {}
        }

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
            isSoftwareRunning = false;
            SaveTaskProgressPercentage = 0.0f;
            pauseEvent = new ManualResetEventSlim();
            cancellationTokenSource = new CancellationTokenSource();
            nFilesUnsavedCancelled = 0;
            SetBindState(ERealTimeState.END);
            IsPriorityLoopOver = false;
            IsNonPriorityLoopOver = false;
        }

        // Overloaded constructor with additional parameters for logging instances.
        internal SaveTask(DirectoryPair CurrentDirectoryPair, LogDaily logDaily, LogRealTime logRealTime, string saveTaskName) : this(CurrentDirectoryPair, saveTaskName)
        {
            this.logDaily = logDaily;
            this.logRealTime = logRealTime;
        }

        // Returns the directory pair associated with the backup task.
        internal DirectoryPair GetDirectoryPair()
        {
            return CurrentDirectoryPair;
        }

        internal abstract ESaveTaskTypes GetSaveTaskType();

        // Start the task
        // Returns true if the task was successful (all files were saved), false otherwise
        // To get the paths of all the files and directories unsaved, call GetUnsavedPaths().
        internal abstract bool Save(SaveTaskManager saveTaskManager);
        // Start the task asynchronously 
        // Returns true if the task was successful (all files were saved), false otherwise
        // To get the paths of all the files and directories unsaved, call GetUnsavedPaths().
        internal async Task<bool> ExecuteSaveAsync(SaveTaskManager saveTaskManager)
        {
            Trace.WriteLine("ESA");
            cancellationTokenSource = new CancellationTokenSource();
            BindSaveTaskProgressPercentage = 0.0f;
            logDaily.CreateDailyFile();
            logRealTime.CreateRealTimeInfo(name, CurrentDirectoryPair.SourcePath, CurrentDirectoryPair.TargetPath, ERealTimeState.WAITING_FOR_PRIORITY_FILES, (int)GetSaveTaskType());
            UnsavedPaths.Clear();

            SetBindState(ERealTimeState.WAITING_FOR_PRIORITY_FILES);
            IsPriorityLoopOver = false;
            IsNonPriorityLoopOver = false;
            Trace.WriteLine("StartedPriority");
            // Set truc en active
            bool SavedEverythingPriority = await Task.Run(() => Save(saveTaskManager), cancellationTokenSource.Token);
            bool SavedEverythingNotPriority = false;
            IsPriorityLoopOver = true;
            if (state != ERealTimeState.ERROR && state != ERealTimeState.PAUSED && state != ERealTimeState.STOPPED)
            {
                SetBindState(ERealTimeState.ACTIVE);
                Trace.WriteLine("StartedNotPriority");
                SavedEverythingNotPriority = await Task.Run(() => Save(saveTaskManager), cancellationTokenSource.Token);
                Trace.WriteLine(SavedEverythingPriority ? "Saved everything for priority" : "saved not everything for priority");
                Trace.WriteLine(SavedEverythingNotPriority ? "Saved everything else" : "saved not everything else");
            }
            IsNonPriorityLoopOver = true;
            logRealTime.UpdateRealTimeProgress(BindState, false, (int)LogUtilities.GetLogFormat());
            return (SavedEverythingPriority && SavedEverythingNotPriority);
        }

        // Method that will be called when a single file is copied (called by recursive and non-recursives)
        // It also handles log calls when a file is copied
        // Can throw unauthorized access exception
        internal void CopySingleFile(string sourcePath, string targetPath, SaveTaskManager saveTaskManager)
        {
            string extension = Path.GetExtension(sourcePath);
            List<string> priorityExtensions = saveTaskManager.GetPriorityExtensions();
            
            // Vérifier si le file extension du fichier copié appartient à la liste des priority extensions et qu'on cherche à sauvegarder les fichiers prioritaires
            if ( (state != ERealTimeState.WAITING_FOR_PRIORITY_FILES && !priorityExtensions.Contains(extension)) ||
                (state == ERealTimeState.WAITING_FOR_PRIORITY_FILES && priorityExtensions.Contains(extension)))
            {
                Trace.WriteLine("Sleeping");
                Thread.Sleep(500);
                Trace.WriteLine("Finished Sleeping");

                if (state == ERealTimeState.STOPPED)
                {
                    Trace.WriteLine("throwExceptionStopped");
                    cancellationTokenSource.Token.ThrowIfCancellationRequested(); // Check if cancellation is requested
                }
                else if (state == ERealTimeState.PAUSED)
                {
                    pauseEvent.Wait(); // This will pause the task if paused
                }
                else if (state != ERealTimeState.ACTIVE && state != ERealTimeState.WAITING_FOR_PRIORITY_FILES)
                {
                    Trace.WriteLine("Invalid state during file copy");
                    throw new Exception("Invalid state during file copy");
                }

                logDaily.stopWatch.Restart(); // Démarrer le chrono pour la copie
                File.Copy(sourcePath, targetPath, true); // Copier le fichier
                logDaily.stopWatch.Stop(); // Arrêter le chrono après la copie

                long encryptionTime = 0; // Par défaut, pas de cryptage

                // Vérifier si le fichier doit être crypté
                if (saveTaskManager.GetEncryptingExtensions().Contains(Path.GetExtension(targetPath)))
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
                        Trace.WriteLine($"Erreur lors du cryptage du fichier {targetPath} : {ex.Message}");
                    }
                }

                // Enregistrer dans le log avec le temps de cryptage
                logDaily.AddDailyInfo(name, sourcePath, targetPath, encryptionTime, (int)LogUtilities.GetLogFormat());
                logRealTime.UpdateRealTimeProgress(BindState, true, (int)LogUtilities.GetLogFormat());

                UpdateProgress();
                Trace.WriteLine($"Successfully copied file, new progress :" + SaveTaskProgressPercentage.ToString());
            }
        }
        // Updates Save task progress
        internal void UpdateProgress()
        {
            int NFilesLeft = logRealTime.realTimeInfo.NbFilesLeftToDo;
            int NTotalFiles = logRealTime.realTimeInfo.TotalFilesToCopy;
            Trace.WriteLine(NTotalFiles.ToString() + " files to copy, " + NFilesLeft.ToString() + " files left");
            if (NTotalFiles == 0) BindSaveTaskProgressPercentage = 100.0f;
            else
                BindSaveTaskProgressPercentage = (float)(NTotalFiles - NFilesLeft) / (float)NTotalFiles * 100.0f;
        }

        internal void CreateDirectory(string directoryPath)
        {

            if (state == ERealTimeState.STOPPED)
                cancellationTokenSource.Token.ThrowIfCancellationRequested(); // Check if cancellation is requested
            else if (state == ERealTimeState.PAUSED)
                pauseEvent.Wait(); // This will pause the task if paused
            else if (state != ERealTimeState.ACTIVE && state!= ERealTimeState.WAITING_FOR_PRIORITY_FILES)
                throw new Exception("Invalid state during file copy");

            DirectoryInfo targetDirectoryInfo = new DirectoryInfo(directoryPath);
            targetDirectoryInfo.Create();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void Pause()
        {
            if (state != ERealTimeState.PAUSED && state != ERealTimeState.STOPPED 
                && state != ERealTimeState.END && state != ERealTimeState.ERROR)
            {
                SetBindState(ERealTimeState.PAUSED);
                logRealTime.UpdateRealTimeProgress(BindState, false, (int)LogUtilities.GetLogFormat());
                pauseEvent.Reset(); // Pause the task
            }
        }

        public void Play()
        {
            if (state == ERealTimeState.PAUSED)
            {
                if(IsPriorityLoopOver && IsNonPriorityLoopOver)
                    SetBindState(ERealTimeState.END);
                else if (!IsPriorityLoopOver)
                    SetBindState(ERealTimeState.WAITING_FOR_PRIORITY_FILES);
                else if (!IsNonPriorityLoopOver)
                    SetBindState(ERealTimeState.ACTIVE);
                logRealTime.UpdateRealTimeProgress(BindState, false, (int)LogUtilities.GetLogFormat());
                pauseEvent.Set(); // Resume the task
            }
        }

        public void Stop()
        {
            if (state != ERealTimeState.STOPPED && state != ERealTimeState.END && state != ERealTimeState.ERROR)
            {
                SetBindState(ERealTimeState.STOPPED);
                logRealTime.UpdateRealTimeProgress(BindState, false, (int)LogUtilities.GetLogFormat());
                cancellationTokenSource.Cancel(); // Request cancellation
            }
        }
    }
}

