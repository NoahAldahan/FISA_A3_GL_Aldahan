using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using EasySaveConsole.Model.Log;
using System.Runtime.CompilerServices;

namespace EasySaveConsole.Model
{
    internal class SaveTaskComplete : SaveTask
    {
        // Constructor
        internal SaveTaskComplete(DirectoryPair CurrentDirectoryPair) : base(CurrentDirectoryPair){}

        // Start a complete save task
        internal override void Save()
        {
            SetRealTimeInfo(CurrentDirectoryPair);
            NotifyLogsCreate();
            FileAttributes sourceAttr = File.GetAttributes(CurrentDirectoryPair.SourcePath);
            FileAttributes targetAttr = File.GetAttributes(CurrentDirectoryPair.TargetPath);
            // if both paths are directories
            if (sourceAttr.HasFlag(FileAttributes.Directory) && targetAttr.HasFlag(FileAttributes.Directory))
            {
                DirectoryInfo sourceDirectoryInfo = new DirectoryInfo(CurrentDirectoryPair.SourcePath);
                DirectoryInfo targetDirectoryInfo = new DirectoryInfo(CurrentDirectoryPair.TargetPath);
                CopyFilesRecursivelyForTwoFolders(sourceDirectoryInfo, targetDirectoryInfo);
            }
            // if the source path is a single file and the target a directory
            else if (!sourceAttr.HasFlag(FileAttributes.Directory) && targetAttr.HasFlag(FileAttributes.Directory))
            {
                StopWatch.Restart();
                string FileName = Path.GetFileName(CurrentDirectoryPair.SourcePath);
                File.Copy(CurrentDirectoryPair.SourcePath, Path.Combine(CurrentDirectoryPair.TargetPath, FileName), true);
                StopWatch.Stop();
                //notify save of a new file
                SetDailyInfo(CurrentDirectoryPair);
                NotifyLogsUpdate();
            }
            // if the target isn't a directory
            else
                throw new Exception("The target path isn't a directory.");
        }

        // Copies files recursively from a source directory to a target directory
        private void CopyFilesRecursivelyForTwoFolders(DirectoryInfo sourceDirectoryInfo, DirectoryInfo targetDirectoryInfo)
        {
            foreach (DirectoryInfo dir in sourceDirectoryInfo.GetDirectories())
                CopyFilesRecursivelyForTwoFolders(dir, targetDirectoryInfo.CreateSubdirectory(dir.Name));
            foreach (FileInfo file in sourceDirectoryInfo.GetFiles())
            {
                StopWatch.Restart();
                file.CopyTo(Path.Combine(targetDirectoryInfo.FullName, file.Name), true);
                StopWatch.Stop();
                //notify save of a new file
                SetDailyInfo(new DirectoryPair(file.FullName, targetDirectoryInfo.FullName + file.Name));
                NotifyLogsUpdate();
            }
        }

        // !!!! Méthode à modifier pour le incrémentiel (vérification des files à copier effectivement et en faire une liste)
        internal override Tuple<int, int> GetTotalFilesToCopy(string path)
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
                    var (subFiles, subSize) = GetTotalFilesToCopy(directory);
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

        internal override void SetRealTimeInfo(DirectoryPair DirectoryPair)
        {
            //notify new Save
            (this.RealTimeInfo.TotalFilesToCopy, this.RealTimeInfo.TotalFilesSize) = GetTotalFilesToCopy(DirectoryPair.SourcePath);
            this.RealTimeInfo.Name = "Name";
            RealTimeInfo.SaveDate = DateTime.Now;
            RealTimeInfo.SourcePath = DirectoryPair.SourcePath;
            RealTimeInfo.TargetPath = DirectoryPair.TargetPath;
            FileInfo fileInfo = new FileInfo(DirectoryPair.SourcePath);
            RealTimeInfo.NbFilesLeftToDo = RealTimeInfo.TotalFilesToCopy;
            RealTimeInfo.Progression = 0;
        }
    }
}
