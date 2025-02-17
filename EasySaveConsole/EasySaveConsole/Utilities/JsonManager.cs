using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using DotNetEnv;
using EasySaveConsole.Model;
using System.Text.Json.Nodes;
using Log;

namespace EasySaveConsole.Utilities
{
    // Utility class for handling JSON operations such as loading settings, messages, and save tasks.
    internal static class JsonManager
    {
        // Paths to various JSON configuration files, loaded from environment variables.
        static private string TranslationPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..",
            Environment.GetEnvironmentVariable("TranslationPath"));

        static private string AppSettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..",
            Environment.GetEnvironmentVariable("AppSettingsPath"));

        static private string SerializationPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..",
            Environment.GetEnvironmentVariable("SerializationPath"));

        static public string LogPathDaily = Path.Combine(Directory.GetCurrentDirectory(), "..", "..",
            Environment.GetEnvironmentVariable("LogPathDaily"));

        static public string LogPathRealTime = Path.Combine(Directory.GetCurrentDirectory(), "..", "..",
            Environment.GetEnvironmentVariable("LogPathRealTime"));

        // Retrieves a translated message from the Translation JSON file based on the specified language.
        static public string GetMessage(string msg, string language)
        {
            try
            {
                // Read and parse JSON file
                string jsonContent = File.ReadAllText(TranslationPath);
                JsonDocument doc = JsonDocument.Parse(jsonContent);
                JsonElement root = doc.RootElement;

                // Retrieve the requested message for the given language
                string value = root.GetProperty(language).GetProperty(msg).GetString();
                return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return default; // Returns null by default if an error occurs
            }
        }

        // Retrieves a setting value from the AppSettings JSON file.
        static public string GetSettings(string settings)
        {
            try
            {
                // Read and parse JSON file
                string jsonContent = File.ReadAllText(AppSettingsPath);
                JsonDocument doc = JsonDocument.Parse(jsonContent);
                JsonElement root = doc.RootElement;
                // Retrieve the requested setting
                string value = root.GetProperty(settings).GetString();
                return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return ""; // Returns an empty string in case of an error
            }
        }

        // Updates the default language setting in the AppSettings JSON file.
        static public bool SetSettings(string value, string key)
        {
            try
            {
                // Read and parse JSON file
                string jsonContent = File.ReadAllText(AppSettingsPath);
                JsonNode jsonNode = JsonNode.Parse(jsonContent);

                // Modify the language setting
                jsonNode[key] = value;

                // Write the modified JSON back to the file
                File.WriteAllText(AppSettingsPath, jsonNode.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));

                return true;
            }
            catch (Exception ex)
            {
                return false; // Return an error message enum in case of failure
            }
        }

        // Saves all save tasks to a JSON file for persistence.
        static public void SerializeSaveTasks(List<SaveTask> SaveTasks)
        {
            try
            {
                // Serialize the list of save tasks to a JSON format
                string jsonContent = JsonSerializer.Serialize(SaveTasks);
                File.WriteAllText(SerializationPath, jsonContent);
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
                jsonContent = File.ReadAllText(SerializationPath);

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
    }
}

