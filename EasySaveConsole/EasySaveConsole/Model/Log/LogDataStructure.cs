using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model.Log
{
    public struct RealTimeInfo
    {
        public string Name { get; set; }
        public string SourceFilePath { get; set; }
        public string TargetFilePath { get; set; }
        public string State { get; set; }  // "END" ou autre état de transfert
        public int TotalFilesToCopy { get; set; }
        public long TotalFilesSize { get; set; }
        public int NbFilesLeftToDo { get; set; }
        public int Progression { get; set; } // En pourcentage

        public RealTimeInfo(string name, string sourceFilePath, string targetFilePath, string state, int totalFilesToCopy, long totalFilesSize, int nbFilesLeftToDo, int progression)
        {
            Name = name;
            SourceFilePath = sourceFilePath;
            TargetFilePath = targetFilePath;
            State = state;
            TotalFilesToCopy = totalFilesToCopy;
            TotalFilesSize = totalFilesSize;
            NbFilesLeftToDo = nbFilesLeftToDo;
            Progression = progression;
        }
    }

        public struct DailyInfo
        {
            public string Name { get; set; }
            public string FileSource { get; set; }
            public string FileTarget { get; set; }
            public long FileSize { get; set; }  // Taille du fichier en octets
            public double FileTransferTime { get; set; }  // Temps de transfert en secondes
            public DateTime DateTime { get; set; }  // Horodatage

        public override string ToString()
        {
            Console.WriteLine($"Nom de sauvegarde : {Name} \n Répertoire source : {FileSource} \n Répertoire cible : {FileTarget} \n Taille du fichier : {FileSize} \n Temps de transfert du fichier : {FileTransferTime} \n Date : {DateTime} ");
            return base.ToString();
        }

        public DailyInfo(string name, string fileSource, string fileTarget, long fileSize, double fileTransferTime, DateTime time)
            {
                Name = name;
                FileSource = fileSource;
                FileTarget = fileTarget;
                FileSize = fileSize;
                FileTransferTime = fileTransferTime;
                DateTime = time;
            }

        }
    }
