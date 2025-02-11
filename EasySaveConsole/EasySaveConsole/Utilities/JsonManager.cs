﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using DotNetEnv;
using EasySaveConsole.Model;
using System.Text.Json.Nodes;

namespace EasySaveConsole.Utilities
{
    internal class JsonManager
    {
        private string TranslationPath;

        private string AppSettingsPath;

        private string SerializationPath;

        public JsonManager() 
        {
            AppSettingsPath = Path.Combine(Directory.GetCurrentDirectory(),"..","..",
            Environment.GetEnvironmentVariable("AppSettingsPath"));

            TranslationPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..",
            Environment.GetEnvironmentVariable("TranslationPath"));

            SerializationPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..",
            Environment.GetEnvironmentVariable("SerializationPath"));
        }

        public string GetMessage(string msg, ELanguage language)
         {
            try
            {
                // Parse JSON
                string jsonContent = File.ReadAllText(TranslationPath);
                JsonDocument doc = JsonDocument.Parse(jsonContent);
                JsonElement root = doc.RootElement;
                string value = root.GetProperty(language.GetValue()).GetProperty(msg).GetString();
                return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return default;
            }
         }

        public string GetSettings(string settings)
        {
            try
            {
                // Parse JSON
                string jsonContent = File.ReadAllText(AppSettingsPath);
                JsonDocument doc = JsonDocument.Parse(jsonContent);
                JsonElement root = doc.RootElement;
                string value = root.GetProperty(settings).GetString();
                return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return "";
            }
        }
        public EMessage SetDefaultLanguage(string languageValue, string languageKey)
        { 
            try
            {
                string jsonContent = File.ReadAllText(AppSettingsPath);
                JsonNode jsonNode = JsonNode.Parse(jsonContent);
                jsonNode[languageKey] = languageValue;
                // Écrire les modifications dans le fichier JSON
                File.WriteAllText(AppSettingsPath, jsonNode.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
                return EMessage.DefaultLanguageChangedSuccessMessage;
            }
            catch (Exception ex) 
            {
                return EMessage.DefaultLanguageChangedErrorMessage;
            }
        }

        // Saves all save tasks config to a json file for persistence
        public void SerializeSaveTasks(List<SaveTask> SaveTasks)
        {
            try
            {
                string jsonContent = JsonSerializer.Serialize(SaveTasks);
                File.WriteAllText(SerializationPath, jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error serializing Save tasks to JSON file: {ex.Message}");
            }
        }

        //Loads all save tasks config from a json file for persistence
        public List<SaveTask> DeserializeSaveTasks()
        {
            string jsonContent = "";
            List<SaveTask> SaveTasks = new List<SaveTask>();
            try
            {
                jsonContent = File.ReadAllText(SerializationPath);
                if (jsonContent == "")
                {
                    return new List<SaveTask>();
                }
                SaveTasks = JsonSerializer.Deserialize<List<SaveTask>>(jsonContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return new List<SaveTask>();
            }
            return SaveTasks;
        }
    }
}
