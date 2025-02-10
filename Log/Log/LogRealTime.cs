using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    public class LogRealTime : Log
    {

        public RealTimeInfo realTimeInfo;

        public LogRealTime(JsonLogManager jsonLogManager) : base(jsonLogManager) { }
        //Create new SaveTask in json file


        public void CreateRealTimeInfo(string SourcePath, string TargetPath, ERealTimeState state)
        {
            //notify new Save
            (realTimeInfo.TotalFilesToCopy, this.realTimeInfo.TotalFilesSize) = GetTotalFilesInfosToCopy(SourcePath);
            this.realTimeInfo.Name = "Name";
            realTimeInfo.SaveDate = DateTime.Now;
            realTimeInfo.SourcePath = SourcePath;
            realTimeInfo.TargetPath = TargetPath;
            FileInfo fileInfo = new FileInfo(SourcePath);
            realTimeInfo.NbFilesLeftToDo = realTimeInfo.TotalFilesToCopy;
            realTimeInfo.State = state.GetValue();
            Console.WriteLine(realTimeInfo.ToString());
            jsonLogManager.AddSaveToRealTimeFile(realTimeInfo);
        }

        public Tuple<int, int> GetTotalFilesInfosToCopy(string path)
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
                totalFiles += files.Length;
                foreach (var file in files)
                {
                    totalFilesSize += (int)new FileInfo(file).Length; // Taille du fichier en octets
                }

                // Récupérer les sous-dossiers et appeler récursivement la fonction
                foreach (string directory in Directory.GetDirectories(path))
                {
                    var (subFiles, subSize) = GetTotalFilesInfosToCopy(directory);
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
            jsonLogManager.UpdateRealTimeProgression(realTimeInfo);
            Console.WriteLine(realTimeInfo.ToString());
        }
    }
}
