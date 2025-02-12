using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    public class LogRealTime : Log
    {

        public RealTimeInfo realTimeInfo;

        public LogRealTime(string LogDailyPath, string LogRealTimePath) : base(LogDailyPath, LogRealTimePath) 
        {}
        //Create new SaveTask in json file


        public void CreateRealTimeInfo(string saveTaskName, string SourcePath, string TargetPath, ERealTimeState state, int saveTaskType)
        {
            //notify new Save
            (realTimeInfo.TotalFilesToCopy, this.realTimeInfo.TotalFilesSize) = GetTotalFilesInfosToCopy(SourcePath, saveTaskType);
            this.realTimeInfo.Name = saveTaskName;
            realTimeInfo.SaveDate = DateTime.Now;
            realTimeInfo.SourcePath = SourcePath;
            realTimeInfo.TargetPath = TargetPath;
            FileInfo fileInfo = new FileInfo(SourcePath);
            realTimeInfo.NbFilesLeftToDo = realTimeInfo.TotalFilesToCopy;
            realTimeInfo.State = state.GetValue();
            Console.WriteLine(realTimeInfo.ToString());
            JsonLogManager.AddSaveToRealTimeFile(realTimeInfo, LogRealTimePath);
        }

        public Tuple<int, int> GetTotalFilesInfosToCopy(string path, int saveTaskType)
        {
            int totalFiles = 0;
            int totalFilesSize = 0;

            try
            {
                // Vérifier si le dossier existe
                if (!Directory.Exists(path))
                {
                    Console.WriteLine("Le chemin spécifié n'existe pas.");
                    return Tuple.Create(0, 0); // Renvoie un tuple avec 0 fichiers et 0 taille
                }

                // Récupérer les fichiers dans le dossier courant et leur taille totale
                string[] files = Directory.GetFiles(path);
                if (saveTaskType == 1)
                {
                    foreach (string file in files)
                    {
                        DateTime lastWriteTime = File.GetLastWriteTime(file);
                        DateTime lastSaveDate = JsonLogManager.GetLastSaveDate(LogDailyPath, file);
                        if (lastWriteTime > lastSaveDate)
                        {
                            totalFiles += 1;
                            totalFilesSize += (int)new FileInfo(file).Length;
                        }
                    }
                }
                else if (saveTaskType == 2)
                {
                    totalFiles += files.Length;
                    foreach (var file in files)
                    {
                        totalFilesSize += (int)new FileInfo(file).Length; // Taille du fichier en octets
                    }
                }

                // Récupérer les sous-dossiers et appeler récursivement la fonction
                foreach (string directory in Directory.GetDirectories(path))
                {
                    var (subFiles, subSize) = GetTotalFilesInfosToCopy(directory, saveTaskType);
                    totalFiles += subFiles;
                    totalFilesSize += subSize;
                }
            }
            
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du comptage des fichiers : {ex.Message}");
                return Tuple.Create(-1, -1); // Indiquer une erreur
            }
            return Tuple.Create(totalFiles, totalFilesSize);
        }

        public void UpdateRealTimeProgress()
        {
            realTimeInfo.NbFilesLeftToDo -= 1;
            realTimeInfo.Progression += ((1.0 / realTimeInfo.TotalFilesToCopy) * 100);
            if (realTimeInfo.NbFilesLeftToDo == 0)
            {
                realTimeInfo.Progression = Convert.ToInt32(realTimeInfo.Progression);
                realTimeInfo.State = ERealTimeState.END.GetValue();
            }
            JsonLogManager.UpdateRealTimeProgression(realTimeInfo, LogRealTimePath);
            Console.WriteLine(realTimeInfo.ToString());
        }
    }
}
