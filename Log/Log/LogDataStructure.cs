using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Log
{

    public enum ERealTimeState
    {
        PAUSED = 0,
        ACTIVE = 1,
        STOPPED = 2,
        END = 3,
        ERROR = 4,
        WAITING_FOR_PRIORITY_FILES = 5,
    }

    public static class ERealTimeStateExstensions
    {
        private static readonly Dictionary<ERealTimeState, string> MessageStrings = new Dictionary<ERealTimeState, string> {
            { ERealTimeState.PAUSED, "Paused" },
            { ERealTimeState.ACTIVE , "Active" },
            { ERealTimeState.STOPPED, "Stopped" },
            { ERealTimeState.END, "END" },
            { ERealTimeState.ERROR , "Error" },
            { ERealTimeState.WAITING_FOR_PRIORITY_FILES , "Waiting for priority files" },
        };

        internal static string GetValue(this ERealTimeState message)
        {
            if (MessageStrings.TryGetValue(message, out var value))
            {
                return value;
            }
            Trace.WriteLine("GeValue exception ");
            throw new ArgumentException($"No string value defined for message: {message}");
        }
    }

    [XmlRoot("RealTimeInfo")]
    public struct RealTimeInfo
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }
        [XmlAttribute("SourcePath")]
        public string SourcePath { get; set; }
        [XmlAttribute("TargetPath")]
        public string TargetPath { get; set; }
        [XmlAttribute("State")]
        public string State { get; set; }  // "END" ou autre état de transfert
        [XmlAttribute("TotalFilesToCopy")]
        public int TotalFilesToCopy { get; set; }
        [XmlAttribute("TotalFilesSize")]
        public long TotalFilesSize { get; set; }
        [XmlAttribute("NbFilesLeftToDo")]
        public int NbFilesLeftToDo { get; set; }
        [XmlAttribute("Progression")]
        public double Progression { get; set; } // En pourcentage
        [XmlAttribute("SaveDate")]
        public DateTime SaveDate { get; set; }

        public RealTimeInfo(string name, string sourcePath, string targetPath, string state, int totalFilesToCopy, long totalFilesSize, int nbFilesLeftToDo, int progression, DateTime saveDate)
        {
            Name = name;
            SourcePath = sourcePath;
            TargetPath = targetPath;
            State = state;
            SaveDate = saveDate;
            TotalFilesToCopy = totalFilesToCopy;
            TotalFilesSize = totalFilesSize;
            NbFilesLeftToDo = nbFilesLeftToDo;
            Progression = progression;
        }

        public void SetState(ERealTimeState newState)
        {
            State  = ERealTimeStateExstensions.GetValue(newState);
        }


        // Method used to debug logs in the CLI
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
            public long EncryptionTimeMs { get; set; } // // Temps de cryptage en millisecondes
            public DateTime DateTime { get; set; }  // Horodatage

        public override string ToString()
        {
            Console.WriteLine(" ============ NEW FILE SAVE ============= ");
            Console.WriteLine($"Nom de sauvegarde : {Name} \n Répertoire source : {FileSource} \n Répertoire cible : {FileTarget} \n Taille du fichier : {FileSize} \n Temps de cryptage : {EncryptionTimeMs}\n Temps de transfert du fichier : {FileTransferTime} \n Date : {DateTime} ");
            Console.WriteLine(" ======================================== ");
            return base.ToString();
        }

        public DailyInfo(string name, string fileSource, string fileTarget, long fileSize, double fileTransferTime, long encryptionTimeMs, DateTime time)
            {
                Name = name;
                FileSource = fileSource;
                FileTarget = fileTarget;
                FileSize = fileSize;
                FileTransferTime = fileTransferTime;
                EncryptionTimeMs = encryptionTimeMs;
                DateTime = time;
            }

        }
    }
