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
        public enum Langages
        {
            EN,
            FR,
            Unknown
        }

        public static class LangagesExtensions
        {
            private static readonly Dictionary<Langages, string> LangageStrings = new Dictionary<Langages, string> {
            { Langages.EN, "EN" },
            { Langages.FR, "FR" },
            {Langages.Unknown, "Unknown" }
            };

            public static string GetValue(this Langages langage)
            {
                if (LangageStrings.TryGetValue(langage, out var value))
                {
                    return value;
                }
                throw new ArgumentException($"No string value defined for langage: {langage}");
            }

        public static Langages GetLangageInstance(string langage)
        {
            foreach (var pair in LangageStrings)
            {
                if (pair.Value.Equals(langage, StringComparison.OrdinalIgnoreCase))
                {
                    return pair.Key;
                }
            }
            throw new ArgumentException($"No Langages enum found for string value: {langage}");
        }

        public static List<string> GetAllStrLangage()
            {
                List<String> strLangages = new List<String>();
                foreach (var langage in LangageStrings)
                {
                    strLangages.Add(langage.Value);
                }
                return strLangages;
            }

        public static Langages ToLangage(string strLangage)
        {
                foreach (var langage in LangageStrings)
                {
                    if (strLangage == langage.Value)
                    {
                        return langage.Key;
                    }
                }
                return Langages.Unknown; // Retourner une valeur par défaut si elle existe
        }
    }
    internal class LangagesManager
    {
        public Langages defaultLangage;
        internal static string langageLibelle;
        private JsonManager jsonManager;
        public LangagesManager()
        {
            langageLibelle = "langage";
            jsonManager = new JsonManager();
            InitDefaultLangage();
        }

        public Messages InitDefaultLangage()
        {
            Langages defaultLangage = LangagesExtensions.ToLangage(jsonManager.GetSettings(langageLibelle));

            if (defaultLangage == Langages.Unknown)
            {
                //return messages error
                return Messages.DefaultLanguageInitErrorMessage;
            }
            this.defaultLangage = defaultLangage;
            return Messages.DefaultLanguageInitSuccessMessage;
        }   

        public static bool IsValideLangage(string langage)
        {
            return LangagesExtensions.GetAllStrLangage().Contains(langage);
        }

        public Messages SetDefaultLangage(string langageValue)
        {
            Messages msg;
            if (IsValideLangage(langageValue))
            {
                msg = jsonManager.SetDefaultLanguage(langageValue, langageLibelle);
            }
            else
            {
               msg = Messages.DefaultLanguageChangedErrorMessage;
            }
            return msg;
        }
        public Langages GetLangageInstance(string langage)
        {
            return LangagesExtensions.GetLangageInstance(langage);
        }
    }
}
