using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using DotNetEnv;
using EasySaveWPFApp.Model;
using System.Text.Json.Nodes;
using Log;
using System.Collections.ObjectModel;
using CryptoSoftLibrary;

namespace EasySaveWPFApp.Utilities
{
    // Utility class for handling JSON operations such as loading settings, messages, and save tasks.
    internal static class JsonManager
    {
        // Paths to various JSON configuration files, loaded from environment variables.
        static private string AppSettingsPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).Parent.FullName,
            Environment.GetEnvironmentVariable("AppSettingsPath"));

        static private string SaveTaskSerializationPath = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).Parent.FullName,
            Environment.GetEnvironmentVariable("SaveTaskSerializationPath"));

        static public string LogPathDaily = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).Parent.FullName,
            Environment.GetEnvironmentVariable("LogPathDaily"));

        static public string LogPathRealTime = Path.Combine(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).Parent.FullName,
            Environment.GetEnvironmentVariable("LogPathRealTime"));

        // TODO Merge and fix .. for this
        static public string EncryptingExtensionsSerializationPath = Path.Combine(Directory.GetCurrentDirectory(), Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).FullName).Parent.FullName,
            Environment.GetEnvironmentVariable("EncryptingExtensionsSerializationPath"));

        static public string EncryptionKey = Environment.GetEnvironmentVariable("EncryptionKey");

        // Retrieves a setting value from the AppSettings JSON file.
        static public string GetSettings(string settings)
        {
            try
            {
                // Read and parse JSON file
                string jsonContent = AsyncFileManager.LockedReadAllText(AppSettingsPath);
                JsonDocument doc = JsonDocument.Parse(jsonContent);
                JsonElement root = doc.RootElement;

                // Retrieve the requested setting
                string value = root.GetProperty(settings).GetString();
                return value;
            }
            catch (Exception ex)
            {
                return ""; // Returns an empty string in case of an error
            }
        }

        // Saves all save tasks to a JSON file for persistence.
        static public void SerializeSaveTasks(ObservableCollection<SaveTask> SaveTasks)
        {
            try
            {
                // Serialize the list of save tasks to a JSON format
                string jsonContent = JsonSerializer.Serialize(SaveTasks);
                AsyncFileManager.LockedWriteAllText(SaveTaskSerializationPath, jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serializing Save tasks to JSON file: {ex.Message}");
            }
        }

        // Loads and deserializes all save tasks from the JSON file.
        static public List<SaveTask> DeserializeSaveTasks()
        {
            string jsonContent = "";
            List<SaveTask> SaveTasks = new List<SaveTask>();

            try
            {
                // Read JSON file content
                jsonContent = AsyncFileManager.LockedReadAllText(SaveTaskSerializationPath);

                // If the file is empty, return an empty list
                if (jsonContent == "")
                {
                    return new List<SaveTask>();
                }

                // Deserialize the JSON into a list of SaveTask objects
                SaveTasks = JsonSerializer.Deserialize<List<SaveTask>>(jsonContent);

                // Reinitialize log instances for each save task after deserialization
                foreach (SaveTask saveTask in SaveTasks)
                {
                    saveTask.SetLogDaily(new LogDaily(LogPathDaily, LogPathRealTime));
                    saveTask.SetLogRealTime(new LogRealTime(LogPathDaily, LogPathRealTime));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return new List<SaveTask>(); // Return an empty list if an error occurs
            }
            return SaveTasks;
        }
        public static void SerializeEncryptingExtensions(List<string> EncryptingExtensions)
        {
            try
            {
                // Serialize the list of save tasks to a JSON format
                string jsonContent = JsonSerializer.Serialize(EncryptingExtensions);
                string path = EncryptingExtensionsSerializationPath;
                AsyncFileManager.LockedWriteAllText(EncryptingExtensionsSerializationPath, jsonContent);
            }
            catch (Exception ex)
            {
                // TODO : Handle this exception with an error popup
                //Console.WriteLine($"Error serializing Save tasks to JSON file: {ex.Message}");
            }
        }
        public static List<string> DeserializeEncryptingExtensions()
        {
            string jsonContent = "";
            List<string> EncryptingExtensions = new List<string>();

            try
            {
                // Read JSON file content
                jsonContent = AsyncFileManager.LockedReadAllText(EncryptingExtensionsSerializationPath); 
                // If the file is empty, return an empty list
                if (jsonContent == "")
                {
                    return EncryptingExtensions;
                }

                // Deserialize the JSON into a list of SaveTask objects
                EncryptingExtensions = JsonSerializer.Deserialize<List<string>>(jsonContent);
                return EncryptingExtensions;
            }
            catch (Exception ex)
            {
                return new List<string>(); // Return an empty list if an error occurs
            }
        }
    }
}

