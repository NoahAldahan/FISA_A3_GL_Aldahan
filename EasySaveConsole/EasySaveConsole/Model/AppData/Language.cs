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
    internal class LanguageExtension : EnumExtension<ELanguage>
    {
        internal LanguageExtension()
        {
            enumStrings.Add(ELanguage.EN, "EN");
            enumStrings.Add(ELanguage.FR, "FR");
            enumStrings.Add(ELanguage.ES, "ES");
            enumStrings.Add(ELanguage.DE, "DE");
            enumStrings.Add(ELanguage.IT, "IT");
            enumStrings.Add(ELanguage.Unknown, "Unknown");
        }
        //Method to convert a string representation to an ELanguage enum value
        internal ELanguage ToLanguage(string strLanguage)
        {
            foreach (var language in enumStrings)
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

        internal LanguageExtension languageExtension;

        // The key used to store the language setting in the configuration
        private static string languageLibelle;

        // Constructor for the LanguageManager class
        internal LanguageManager(LanguageExtension languageExtension)
        {
            languageLibelle = "language";
            this.languageExtension = languageExtension;
            InitDefaultLanguage(); // Initialize the default language
        }

        internal string getStrDefaultLanguage()
        {
            return languageExtension.GetValue(defaultLanguage);
        }

        // Method to initialize the default language from the configuration
        internal EMessage InitDefaultLanguage()
        {
            ELanguage defaultLanguage = languageExtension.ToLanguage(JsonManager.GetSettings(languageLibelle));

            if (defaultLanguage == ELanguage.Unknown)
            {
                // Return an error message if the default language is not recognized
                return EMessage.DefaultLanguageInitErrorMessage;
            }
            this.defaultLanguage = defaultLanguage;
            return EMessage.DefaultLanguageInitSuccessMessage;
        }

        // Method to set the default language
        internal EMessage SetDefaultLanguage(string languageValue)
        {
            EMessage msg;
            if (languageExtension.IsValidStrValue(languageValue))
            {
                if(JsonManager.SetSettings(languageValue, languageLibelle)) 
                {
                    defaultLanguage = languageExtension.GetInstance(languageValue);
                    return EMessage.DefaultLanguageChangedSuccessMessage;
                }
                return EMessage.DefaultLanguageChangedErrorMessage;
            }
            else
            {
                msg = EMessage.DefaultLanguageChangedErrorMessage;
            }
            return msg;
        }
    }
}

