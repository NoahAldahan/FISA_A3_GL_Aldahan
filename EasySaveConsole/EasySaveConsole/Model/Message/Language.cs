using EasySaveConsole.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Configuration;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Utilities
{
    // Enum representing supported languages
    internal enum ELanguage
    {
        EN, // English
        FR, // Français
        ES, // Español
        DE, // Deutsch
        IT, // Italiano
        Unknown // Langue inconnue
    }

    // Static class providing utility methods for language management
    internal static class LanguageExtension
        {
            private static readonly Dictionary<ELanguage, string> LanguageStrings = new Dictionary<ELanguage, string>
            {
                { ELanguage.EN, "EN" },
                { ELanguage.FR, "FR" },
                { ELanguage.ES, "ES" },
                { ELanguage.DE, "DE" },
                { ELanguage.IT, "IT" },
                { ELanguage.Unknown, "Unknown" }
            };
        internal static string GetValue(this ELanguage language)
        {
            if (LanguageStrings.TryGetValue(language, out var value))
            {
                return value;
            }
            throw new ArgumentException($"No string value defined for language: {language}");
        }

        // Method to get the ELanguage enum value from its string representation
        internal static ELanguage GetLanguageInstance(string language)
        {
            foreach (var pair in LanguageStrings)
            {
                if (pair.Value.Equals(language, StringComparison.OrdinalIgnoreCase))
                {
                    return pair.Key;
                }
            }
            throw new ArgumentException($"No Languages enum found for string value: {language}");
        }

        // Method to get a list of all string representations of supported languages
        internal static List<string> GetAllStrLanguage()
        {
            List<string> strLanguages = new List<string>();
            foreach (var language in LanguageStrings)
            {
                strLanguages.Add(language.Value);
            }
            return strLanguages;
        }

        // Method to convert a string representation to an ELanguage enum value
        internal static ELanguage ToLanguage(string strLanguage)
        {
            foreach (var language in LanguageStrings)
            {
                if (strLanguage == language.Value)
                {
                    return language.Key;
                }
            }
            return ELanguage.Unknown; // Return a default value if the language is not found
        }
    }

    // Class for managing the default language settings
    internal class LanguageManager
    {
        // The default language setting
        internal ELanguage defaultLanguage;

        // The key used to store the language setting in the configuration
        private static string languageLibelle;

        // Constructor for the LanguageManager class
        internal LanguageManager()
        {
            languageLibelle = "language";
            InitDefaultLanguage(); // Initialize the default language
        }

        // Method to initialize the default language from the configuration
        internal EMessage InitDefaultLanguage()
        {
            ELanguage defaultLanguage = LanguageExtension.ToLanguage(JsonManager.GetSettings(languageLibelle));

            if (defaultLanguage == ELanguage.Unknown)
            {
                // Return an error message if the default language is not recognized
                return EMessage.DefaultLanguageInitErrorMessage;
            }
            this.defaultLanguage = defaultLanguage;
            return EMessage.DefaultLanguageInitSuccessMessage;
        }

        // Method to check if a given language string is valid
        internal static bool IsValideLanguage(string language)
        {
            return LanguageExtension.GetAllStrLanguage().Contains(language);
        }

        // Method to set the default language
        internal EMessage SetDefaultLanguage(string languageValue)
        {
            EMessage msg;
            if (IsValideLanguage(languageValue))
            {
                msg = JsonManager.SetDefaultLanguage(languageValue, languageLibelle);
                defaultLanguage = LanguageExtension.GetLanguageInstance(languageValue);
            }
            else
            {
                msg = EMessage.DefaultLanguageChangedErrorMessage;
            }
            return msg;
        }

        // Method to get the ELanguage enum value from its string representation
        internal ELanguage GetLanguageInstance(string language)
        {
            return LanguageExtension.GetLanguageInstance(language);
        }
    }
}

