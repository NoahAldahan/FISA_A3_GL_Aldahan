using EasySaveConsole.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model.Log
{
    // Enum representing supported languages
    internal enum ELogSaveTaskType
    {
        JSON,
        XML,
        Unknown
    }
    internal class LogSaveTaskTypeExtension : EnumExtension<ELogSaveTaskType>
    {
        internal LogSaveTaskTypeExtension()
        {
            enumStrings.Add(ELogSaveTaskType.XML, "XML");
            enumStrings.Add(ELogSaveTaskType.JSON, "JSON");
            enumStrings.Add(ELogSaveTaskType.Unknown, "Unknown");
        }
        // Method to convert a string representation to an ELogSaveTaskType enum value
        internal ELogSaveTaskType ToSaveTaskType(string strLogSaveTaskType)
        {
            foreach (var logSaveTaskType in enumStrings)
            {
                if (strLogSaveTaskType == logSaveTaskType.Value)
                {
                    return logSaveTaskType.Key;
                }
            }
            return ELogSaveTaskType.Unknown; // Return a default value if the language is not found
        }
    }

    // Class for managing the default language settings
    internal class LogManager
    {
        // The default language setting
        internal ELogSaveTaskType defaultSaveTaskType;
        internal LogSaveTaskTypeExtension logExtension;

        // The key used to store the language setting in the configuration
        private static string logLibelle;

        // Constructor for the LanguageManager class
        internal LogManager(LogSaveTaskTypeExtension logExtension)
        {
            logLibelle = "logSaveTaskType";
            this.logExtension = logExtension;
            InitDefaultSaveTaskType();
        }

        internal string getStrDefaultSaveTaskType()
        {
            return logExtension.GetValue(defaultSaveTaskType);
        }

        // Method to initialize the default language from the configuration
        internal EMessage InitDefaultSaveTaskType()
        {
            ELogSaveTaskType defaultSaveTaskType = logExtension.ToSaveTaskType(JsonManager.GetSettings(logLibelle));
            return SetDefaultLog(logExtension.GetValue(defaultSaveTaskType));
        }

        // Method to set the default Log
        internal EMessage SetDefaultLog(string saveTaskTypeValue)
        {
            bool result;
            if (logExtension.IsValidStrValue(saveTaskTypeValue))
            {
                result = JsonManager.SetSettings(saveTaskTypeValue, logLibelle);
                if (result) 
                {
                    defaultSaveTaskType = logExtension.GetInstance(saveTaskTypeValue);
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            return result ? EMessage.LogSaveTaskTypeChangeSuccessMessage : EMessage.LogSaveTaskTypeChangeErrorMessage;

        }
    }
}
