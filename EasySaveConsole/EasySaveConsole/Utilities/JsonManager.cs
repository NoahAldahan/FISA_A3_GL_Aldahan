using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;
using DotNetEnv;

namespace EasySaveConsole.Utilities
{
    internal class JsonManager
    {
        string AppSettingsPath = Path.Combine(AppContext.BaseDirectory, Environment.GetEnvironmentVariable("AppSettingsPath"));
        string TranslationPath = Path.Combine(AppContext.BaseDirectory, Environment.GetEnvironmentVariable("TranslationPath"));
        
        public Langages GetDefaultLangage()
        {
            return Langages.EN;
        }
        public string GetMessage(Messages msg, Langages langage)
         {
            try
            {
                // Parse JSON
                string jsonContent = File.ReadAllText(TranslationPath);
                JsonDocument doc = JsonDocument.Parse(jsonContent);
                JsonElement root = doc.RootElement;
                string value = root.GetProperty(langage.GetValue()).GetProperty(msg.GetValue()).GetString();
                return value;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading JSON file: {ex.Message}");
                return default;
            }
         }
        public bool SetDefaultLanguage(string langage)
        { 
            Console.WriteLine(AppSettingsPath);
            return true;
        }
    }
}