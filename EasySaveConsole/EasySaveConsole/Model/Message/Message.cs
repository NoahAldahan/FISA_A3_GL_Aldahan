using EasySaveConsole.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model
{
    // Enum representing various messages used in the application
    public enum EMessage
    {
        // Base CLI Messages
        MenuMessage,                     // Message displayed for the main menu
        StopMessage,                     // Message displayed when stopping the application
        ErrorUserEntryStrMessage,        // Error message for invalid string input
        ErrorUserEntryOptionMessage,     // Error message for invalid option input
        PressKeyToContinue,              // Message prompting the user to press a key to continue

        // Language-related Messages
        LanguagesListMessage,            // Message displaying the list of available languages
        LanguageChangeSuccessMessage,    // Message indicating successful language change
        DefaultLanguageInitSuccessMessage, // Message indicating successful initialization of the default language
        DefaultLanguageInitErrorMessage, // Error message for failed initialization of the default language
        DefaultLanguageChangedSuccessMessage, // Message indicating successful change of the default language
        DefaultLanguageChangedErrorMessage, // Error message for failed change of the default language
        AskLanguageMessage,              // Message asking the user to select a language
        MenuLanguageMessage,             // Message displayed for the language menu

        // Save Task-related Messages
        StartSaveTaskMessage,            // Message indicating the start of save tasks
        ShowSaveTaskRegisterMessage,     // Message displaying the list of registered save tasks
        CreateSaveTaskMessage,           // Message indicating the creation of a new save task
        MenuSaveTaskMessage,             // Message displayed for the save task menu
        AskSaveTaskNameMessage,          // Message asking the user to enter the save task name
        AskSaveTaskTargetFolderMessage,  // Message asking the user to enter the target folder for the save task
        AskSaveTaskSourceFolderMessage,  // Message asking the user to enter the source folder for the save task
        AskSaveTaskType,                 // Message asking the user to enter the type of save task
        ErrorSaveTaskPathMessage,        // Error message for invalid save task path
        ErrorSaveTaskTypeMessage,        // Error message for invalid save task type
        SaveTaskAddSuccessMessage        // Message indicating successful addition of a save task
    }

    // Static class providing utility methods for message management
    public static class MessageExtensions
    {
        // Dictionary mapping EMessage enum values to their string representations
        private static readonly Dictionary<EMessage, string> MessageStrings = new Dictionary<EMessage, string>
        {
            // Base CLI Messages
            { EMessage.MenuMessage, "MenuMessage" },
            { EMessage.StopMessage, "StopMessage" },
            { EMessage.PressKeyToContinue, "PressKeyToContinue" },
            { EMessage.ErrorUserEntryStrMessage, "ErrorUserEntryStrMessage" },
            { EMessage.ErrorUserEntryOptionMessage, "ErrorUserEntryOptionMessage" },

            // Language-related Messages
            { EMessage.LanguagesListMessage, "LanguagesListMessage" },
            { EMessage.LanguageChangeSuccessMessage, "LanguageChangeSuccessMessage" },
            { EMessage.DefaultLanguageInitSuccessMessage, "DefaultLanguageInitSuccessMessage" },
            { EMessage.DefaultLanguageInitErrorMessage, "DefaultLanguageInitErrorMessage" },
            { EMessage.DefaultLanguageChangedSuccessMessage, "DefaultLanguageChangedSuccessMessage" },
            { EMessage.DefaultLanguageChangedErrorMessage, "DefaultLanguageChangedErrorMessage" },
            { EMessage.AskLanguageMessage, "AskLanguageMessage" },
            { EMessage.MenuLanguageMessage, "MenuLanguageMessage" },

            // Save Task-related Messages
            { EMessage.StartSaveTaskMessage, "StartSaveTaskMessage" },
            { EMessage.CreateSaveTaskMessage, "CreateSaveTaskMessage" },
            { EMessage.MenuSaveTaskMessage, "MenuSaveTaskMessage" },
            { EMessage.AskSaveTaskNameMessage, "AskSaveTaskNameMessage" },
            { EMessage.AskSaveTaskTargetFolderMessage, "AskSaveTaskTargetFolderMessage" },
            { EMessage.AskSaveTaskSourceFolderMessage, "AskSaveTaskSourceFolderMessage" },
            { EMessage.AskSaveTaskType, "AskSaveTaskType" },
            { EMessage.ErrorSaveTaskPathMessage, "ErrorSaveTaskPathMessage" },
            { EMessage.ErrorSaveTaskTypeMessage, "ErrorSaveTaskTypeMessage" },
            { EMessage.SaveTaskAddSuccessMessage, "SaveTaskAddSuccessMessage" },
            { EMessage.ShowSaveTaskRegisterMessage, "ShowSaveTaskRegisterMessage" }
        };

        // Extension method to get the string representation of an EMessage enum value
        internal static string GetValue(this EMessage message)
        {
            if (MessageStrings.TryGetValue(message, out var value))
            {
                return value;
            }
            throw new ArgumentException($"No string value defined for message: {message}");
        }
    }

    // Class for managing messages and their translations
    internal class MessageManager
    {
        // Manager for handling language settings
        LanguageManager languageManager;

        // Constructor for the MessageManager class
        public MessageManager(LanguageManager languageManager)
        {
            this.languageManager = languageManager;
        }

        // Method to get the translated message based on the current language setting
        public string GetMessageTranslate(EMessage message)
        {
            return JsonManager.GetMessage(message.GetValue(), languageManager.defaultLanguage);
        }
    }
}
