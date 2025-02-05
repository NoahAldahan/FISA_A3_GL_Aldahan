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
    internal enum ELanguage
    {
        EN, // English
        FR, // Français
        ES, // Español
        DE, // Deutsch
        IT, // Italiano
        PT, // Português
        NL, // Nederlands
        Unknown // Langue inconnue
    }

    internal static class LanguageExtension
        {
            private static readonly Dictionary<ELanguage, string> LanguageStrings = new Dictionary<ELanguage, string>
            {
                { ELanguage.EN, "EN" },
                { ELanguage.FR, "FR" },
                { ELanguage.ES, "ES" },
                { ELanguage.DE, "DE" },
                { ELanguage.IT, "IT" },
                { ELanguage.PT, "PT" },
                { ELanguage.NL, "NL" },
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

        internal static List<string> GetAllStrLanguage()
            {
                List<String> strLanguages = new List<String>();
                foreach (var language in LanguageStrings)
                {
                    strLanguages.Add(language.Value);
                }
                return strLanguages;
            }

        internal static ELanguage ToLanguage(string strLanguage)
        {
                foreach (var language in LanguageStrings)
                {
                    if (strLanguage == language.Value)
                    {
                        return language.Key;
                    }
                }
                return ELanguage.Unknown; // Retourner une valeur par défaut si elle existe
        }
    }
    internal class LanguageManager
    {
        internal ELanguage defaultLanguage;
        private static string languageLibelle;
        private JsonManager jsonManager;
        internal LanguageManager()
        {
            languageLibelle = "language";
            jsonManager = new JsonManager();
            InitDefaultLanguage();
        }

        internal EMessage InitDefaultLanguage()
        {
            ELanguage defaultLanguage = LanguageExtension.ToLanguage(jsonManager.GetSettings(languageLibelle));

            if (defaultLanguage == ELanguage.Unknown)
            {
                //return messages error
                return EMessage.DefaultLanguageInitErrorMessage;
            }
            this.defaultLanguage = defaultLanguage;
            return EMessage.DefaultLanguageInitSuccessMessage;
        }

        internal static bool IsValideLanguage(string language)
        {
            return LanguageExtension.GetAllStrLanguage().Contains(language);
        }

        internal EMessage SetDefaultLanguage(string languageValue)
        {
            EMessage msg;
            if (IsValideLanguage(languageValue))
            {
                msg = jsonManager.SetDefaultLanguage(languageValue, languageLibelle);
                defaultLanguage = LanguageExtension.GetLanguageInstance(languageValue);
            }
            else
            {
               msg = EMessage.DefaultLanguageChangedErrorMessage;
            }
            return msg;
        }
        internal ELanguage GetLanguageInstance(string language)
        {
            return LanguageExtension.GetLanguageInstance(language);
        }
    }
}
