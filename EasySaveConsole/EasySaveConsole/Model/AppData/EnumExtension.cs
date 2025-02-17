using EasySaveConsole.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model
{
    internal class EnumExtension<TEnum> where TEnum : Enum
    {
        // Dictionnaire pour stocker les valeurs associées aux enums
        internal static Dictionary<TEnum, string> enumStrings = new Dictionary<TEnum, string>() { };
            
        // Méthode d'extension pour récupérer la valeur string associée à une instance d'énumération
        public string GetValue(TEnum enumValue)
        {
            if (enumStrings.TryGetValue(enumValue, out var value))
            {
                return value;
            }
            throw new ArgumentException($"No string value defined for {enumValue}.");
        }

        // Method to check if a given language string is valid
        public bool IsValidStrValue(string ValueStr)
        {
            return GetAllStrInstance().Contains(ValueStr);
        }

        // Méthode pour récupérer l'instance d'enum à partir de la valeur string
        public TEnum GetInstance(string stringValue)
        {
            foreach (var pair in enumStrings)
            {
                if (pair.Value.Equals(stringValue, StringComparison.OrdinalIgnoreCase))
                {
                    return pair.Key;
                }
            }
            throw new ArgumentException($"No enum found for string value: {stringValue}");
        }

        // Method to get a list of all string representations of supported languages
        internal List<string> GetAllStrInstance()
        {
            List<string> strValue = new List<string>();
            foreach (var instance in enumStrings)
            {
                strValue.Add(instance.Value);
            }
            return strValue;
        }
    }
}