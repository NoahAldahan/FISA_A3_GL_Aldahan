using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Log
{
    internal class XmlLogManager
    {
        internal static void UpdateRealTimeProgression(RealTimeInfo realTimeInfo, string LogRealTimePath)
        {
            List<RealTimeInfo> xmlObjectList = new List<RealTimeInfo>();
            string fileName = GetFileRealTimeName(LogRealTimePath);
            if (File.Exists(fileName))
            {
                try
                {
                    xmlObjectList = DeserializeXml<List<RealTimeInfo>>(fileName) ?? new List<RealTimeInfo>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors du chargement du XML : {ex.Message}");
                }
            }
            int index = xmlObjectList.FindIndex(rt => rt.Name == realTimeInfo.Name);
            if (index != -1)
            {
                xmlObjectList[index] = realTimeInfo;
            }
            SerializeXml(fileName, xmlObjectList);
        }

        internal static void CreateRepertories(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        internal static string GetFileDailyName(DateTime Date, string LogDailyPath)
        {
            return $"{LogDailyPath}backup_{Date:yyyy-MM-dd}.xml";
        }

        internal static string GetFileRealTimeName(string LogRealTimePath)
        {
            return $"{LogRealTimePath}RealTimeSave.xml";
        }

        internal static void CreateDailyXmlFile(DateTime Date, string LogDailyPath)
        {
            string fileName = GetFileDailyName(Date, LogDailyPath);
            try
            {
                if (!File.Exists(fileName))
                {
                    File.Create(fileName).Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating XML file: {ex.Message}");
            }
        }

        internal static void CreateRealTimeXmlFile(string LogRealTimePath)
        {
            string fileName = GetFileRealTimeName(LogRealTimePath);
            try
            {
                if (!File.Exists(fileName))
                {
                    File.Create(fileName).Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating XML file: {ex.Message}");
            }
        }

        internal static void AddSaveToDailyFile(DailyInfo dailyInfo, string LogDailyPath)
        {
            string dailyInfoPath = GetFileDailyName(dailyInfo.DateTime, LogDailyPath);
            AddXmlLogObject(dailyInfoPath, dailyInfo);
        }

        internal static void AddSaveToRealTimeFile(RealTimeInfo realTimeInfo, string LogRealTimePath)
        {
            AddXmlLogObject(GetFileRealTimeName(LogRealTimePath), realTimeInfo);
        }

        internal static void AddXmlLogObject<T>(string FilePath, T LogObject)
        {
            List<T> xmlObjectList = new List<T>();
            if (File.Exists(FilePath))
            {
                try
                {
                    xmlObjectList = DeserializeXml<List<T>>(FilePath) ?? new List<T>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erreur lors du chargement du XML : {ex.Message}");
                }
            }

            if(LogObject.GetType() == typeof(RealTimeInfo))
            {
                var propertyInfo = typeof(T).GetProperty("Name");

                if (propertyInfo != null)
                {
                    // Get the value of the Name property of LogObject
                    var logObjectName = propertyInfo.GetValue(LogObject);

                    // Remove any existing object with the same Name
                    xmlObjectList.RemoveAll(obj => propertyInfo.GetValue(obj).Equals(logObjectName));
                }
            }
            xmlObjectList.Add(LogObject);
            SerializeXml(FilePath, xmlObjectList);
        }

        private static void SerializeXml<T>(string filePath, T data)
        {
            try
            {
                using (var writer = new StreamWriter(filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(writer, data);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'écriture du XML : {ex.Message}");
            }
        }

        private static T DeserializeXml<T>(string filePath) where T : class
        {
            try
            {
                using (var reader = new StreamReader(filePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(reader) as T;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la lecture du XML : {ex.Message}");
                return null;
            }
        }

        internal static DateTime GetLastSaveDateFromXml(string LogDailyPath, string FilePath)
        {
            DirectoryInfo LogDailyDirectory = new DirectoryInfo(LogDailyPath);
            try
            {
                foreach (var file in LogDailyDirectory.GetFiles("*.xml").OrderByDescending(f => f.CreationTime))
                {
                    List<DailyInfo> entities = DeserializeXml<List<DailyInfo>>(file.FullName);
                    DailyInfo foundEntity = entities.Find(e => e.FileSource == FilePath);
                    if (foundEntity.DateTime != null)
                    {
                        return foundEntity.DateTime;
                    }
                }
                return DateTime.MinValue;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la recherche de dernière sauvegarde. {ex}");
                return DateTime.MinValue;
            }
        }
    }
}

