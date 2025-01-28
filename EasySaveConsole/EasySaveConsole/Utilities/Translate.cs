using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace EasySaveConsole.Utilities
{

    public enum Langages
    {
        EN,
        FR
    }

    public static class LangagesExtensions
    {
        private static readonly Dictionary<Langages, string> dictionary = new Dictionary<Langages, string> {
        { Langages.EN, "EN" },
        { Langages.FR, "FR" }
    };
        private static readonly Dictionary<Langages, string> LangageStrings = dictionary;

        public static string GetValue(this Langages langage)
        {
            return LangageStrings[langage];
        }
        public static List<string> GetAllStrLangage()
        {
            List<String> strLangages = new List<String>();
            foreach (var langage in LangageStrings) {
                strLangages.Add(langage.Value);
            }
            return strLangages;
        }
    }


    class LangagesManager
    {

        public JsonManager jsonManager;
        public Langages defaultLangage;
        public LangagesManager()
        {
            jsonManager = new JsonManager();
            defaultLangage = jsonManager.GetDefaultLangage();
        }

        public static bool IsValideLangage(string langage)
        {
            return LangagesExtensions.GetAllStrLangage().Contains(langage);
        }

        public string SetDefaultLangage(string langage)
        {
            if (!IsValideLangage(langage))
            {
                bool result = jsonManager.SetDefaultLanguage(langage);
                return result ? "Le langage a bien été ajouté par défaut" : "Une erreur est surevenue";
            }
            return "Le langage saisie n'est pas valide";
        }
    }
}
