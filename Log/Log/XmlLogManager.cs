using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Log
{
    public class XmlLogManager
    {
        private static readonly object SerializeLock = new object();
        internal static void UpdateRealTimeProgression(RealTimeInfo realTimeInfo, string LogRealTimePath)
        {
            List<RealTimeInfo> xmlObjectList = new List<RealTimeInfo>();
            string fileName = GetFileRealTimeName(LogRealTimePath);
            if (File.Exists(fileName))
            {
                try
                {
                    xmlObjectList = LockedXmlDeserialize<List<RealTimeInfo>>(fileName) ?? new List<RealTimeInfo>();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("UpdateRealTimeProgression : " + ex.Message);
                    throw new Exception("Log XML UpdateRealTimeProgression");
                }
            }
            int index = xmlObjectList.FindIndex(rt => rt.Name == realTimeInfo.Name);
            if (index != -1)
            {
                xmlObjectList[index] = realTimeInfo;
            }
            else if (index == -1)
            {
                xmlObjectList.Add(realTimeInfo);
            }
            LockedXmlSerialize(fileName, xmlObjectList);
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
                Trace.WriteLine("CreateDailyXmlFile : " + ex.Message);
                throw new Exception("Log XML CreateDailyXmlFile");
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
                Trace.WriteLine("CreateRealTimeXmlFile : " + ex.Message);
                throw new Exception("Log XML CreateRealTimeXmlFile");
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
                    xmlObjectList = LockedXmlDeserialize<List<T>>(FilePath) ?? new List<T>();
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("AddXmlLogObject : " + ex.Message);
                    throw new Exception("Log XML AddXmlLogObject");
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
            LockedXmlSerialize(FilePath, xmlObjectList);
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
                Trace.WriteLine("Log XML SerializeXml : " + ex.Message);
                throw new Exception("Log XML SerializeXml");
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
                Trace.WriteLine("DeserializeXml : " + ex.Message);
                throw new Exception("Log XML DeserializeXml");
            }
        }

        public static DateTime GetLastSaveDateFromXml(string LogDailyPath, string FilePath)
        {
            DirectoryInfo LogDailyDirectory = new DirectoryInfo(LogDailyPath);
            try
            {
                foreach (var file in LogDailyDirectory.GetFiles("*.xml").OrderByDescending(f => f.CreationTime))
                {
                    List<DailyInfo> entities = LockedXmlDeserialize<List<DailyInfo>>(file.FullName);
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
                Trace.WriteLine($"Erreur lors de la recherche de dernière sauvegarde. {ex}");
                return DateTime.MinValue;
            }
        }
        public static void LockedXmlSerialize<T>(string filePath, T data) where T : class
        {
            Trace.WriteLine("before locked xml");
            lock (SerializeLock)
            {
                SerializeXml<T>(filePath, data);
            }
        }
        public static T LockedXmlDeserialize<T>(string jsonContent) where T : class
        {
            lock (SerializeLock)
            {
                return DeserializeXml<T>(jsonContent);
            }
        }
    }
}

