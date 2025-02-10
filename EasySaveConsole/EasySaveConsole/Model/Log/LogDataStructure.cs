using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model.Log
{

    public enum ERealTimeState
    {
        ACTIVE = 0,
        END = 1,
        ERROR = 2,
    }

    public static class ERealTimeStateExstensions
    {
        private static readonly Dictionary<ERealTimeState, string> MessageStrings = new Dictionary<ERealTimeState, string> {
            { ERealTimeState.ACTIVE, "ACTIVE" },
            { ERealTimeState.END, "END" },
            {ERealTimeState.ERROR , "Error" },
        };

        internal static string GetValue(this ERealTimeState message)
        {
            if (MessageStrings.TryGetValue(message, out var value))
            {
                return value;
            }
            throw new ArgumentException($"No string value defined for message: {message}");
        }
    }


    public struct RealTimeInfo
    {
        public string Name { get; set; }
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
        public string State { get; set; }  // "END" ou autre état de transfert
        public int TotalFilesToCopy { get; set; }
        public long TotalFilesSize { get; set; }
        public int NbFilesLeftToDo { get; set; }
        public double Progression { get; set; } // En pourcentage
        public DateTime SaveDate { get; set; }

        public RealTimeInfo(string name, string sourceFilePath, string targetFilePath, string state, int totalFilesToCopy, long totalFilesSize, int nbFilesLeftToDo, int progression, DateTime saveDate)
        {
            Name = name;
            SourcePath = sourceFilePath;
            TargetPath = targetFilePath;
            State = state;
            SaveDate = saveDate;
            TotalFilesToCopy = totalFilesToCopy;
            TotalFilesSize = totalFilesSize;
            NbFilesLeftToDo = nbFilesLeftToDo;
            Progression = progression;
        }

        public override string ToString()
        {
            Console.WriteLine(" ============ NEW SAVE ============= ");
            Console.WriteLine($"Nom de sauvegarde : {Name} \n Répertoire source : {SourcePath} \n Répertoire cible : {TargetPath} \n Taille du fichier : {State} \n Temps de transfert du fichier : {TotalFilesToCopy} \n Date : {SaveDate} \n " +
                $"TotalFilesToCopy : {TotalFilesToCopy}  \n TotalFilesSize : {TotalFilesSize} \n  NbFilesLeftToDo : {NbFilesLeftToDo} \n  Progression : {Progression} ");
            Console.WriteLine(" =================================== ");
            return base.ToString();
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
            Console.WriteLine(" ============ NEW FILE SAVE ============= ");
            Console.WriteLine($"Nom de sauvegarde : {Name} \n Répertoire source : {FileSource} \n Répertoire cible : {FileTarget} \n Taille du fichier : {FileSize} \n Temps de transfert du fichier : {FileTransferTime} \n Date : {DateTime} ");
            Console.WriteLine(" ======================================== ");
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
