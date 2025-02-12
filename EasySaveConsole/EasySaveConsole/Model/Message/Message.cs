using EasySaveConsole.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model
{
        public enum EMessage
        {
            //Based Cli Message
            ErrorStartScreenLoading,
            MenuMessage,
            StopMessage,
            ErrorUserEntryStrMessage,
            ErrorUserEntryOptionMessage,
            PressKeyToContinue,
            //Language
            LanguagesListMessage,
            LanguageChangeSuccessMessage,
            DefaultLanguageInitSuccessMessage,
            DefaultLanguageInitErrorMessage,
            DefaultLanguageChangedSuccessMessage,
            DefaultLanguageChangedErrorMessage,
            AskLanguageMessage,
            MenuLanguageMessage,
            //SaveTask
            StartSaveTaskMessage,
            ShowSaveTaskRegisterMessage,
            CreateSaveTaskMessage,
            MenuSaveTaskMessage,
            AskSaveTaskNameMessage,
            AskSaveTaskTargetFolderMessage,
            AskSaveTaskSourceFolderMessage,
            AskSaveTaskType,
            ErrorSaveTaskPathMessage,
            ErrorSaveTaskTypeMessage,
            SaveTaskAddSuccessMessage

    }

        public static class MessageExtensions
        {
            private static readonly Dictionary<EMessage, string> MessageStrings = new Dictionary<EMessage, string> {
            //Based Cli Message
            { EMessage.ErrorStartScreenLoading, "ErrorStartScreenLoading" },
            { EMessage.MenuMessage, "MenuMessage" },
            { EMessage.StopMessage, "StopMessage" },
            { EMessage.PressKeyToContinue, "PressKeyToContinue" },
            { EMessage.ErrorUserEntryStrMessage, "ErrorUserEntryStrMessage" },
            { EMessage.ErrorUserEntryOptionMessage, "ErrorUserEntryOptionMessage" },
            //Language
            { EMessage.LanguagesListMessage, "LanguagesListMessage" },
            { EMessage.LanguageChangeSuccessMessage, "LanguageChangeSuccessMessage" },
            { EMessage.DefaultLanguageInitSuccessMessage, "DefaultLanguageInitSuccessMessage" },
            { EMessage.DefaultLanguageInitErrorMessage, "DefaultLanguageInitErrorMessage" },
            { EMessage.DefaultLanguageChangedSuccessMessage, "DefaultLanguageChangedSuccessMessage" },
            { EMessage.DefaultLanguageChangedErrorMessage, "DefaultLanguageChangedErrorMessage" },
            { EMessage.AskLanguageMessage, "AskLanguageMessage" },
            { EMessage.MenuLanguageMessage, "MenuLanguageMessage" },
            //SaveTask
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

            internal static string GetValue(this EMessage message)
            {
                if (MessageStrings.TryGetValue(message, out var value))
                {
                    return value;
                }
                throw new ArgumentException($"No string value defined for message: {message}");
            }
        }
        internal class MessageManager
        {
            LanguageManager languageManager;
            public MessageManager(LanguageManager languageManager)
            {
                this.languageManager = languageManager;
            }
            public string GetMessageTranslate(EMessage message)
            {   
                return JsonManager.GetMessage(message.GetValue(), languageManager.defaultLanguage);
            }
        }
}
