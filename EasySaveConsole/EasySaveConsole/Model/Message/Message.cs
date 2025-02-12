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
            ErrorMessage,
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
            SaveTaskAddSuccessMessage,
            ErrorSaveTaskNotFoundMessage,
            SuccessStartSaveTaskMessage,
            SuccessModifySaveTaskMessage,
            SuccessSuppressSaveTaskMessage,
            ErrorModifySaveTaskMessage,
            ErrorSuppressSaveTaskMessage,
            ErrorStartEndIndexSaveTaskMessage,
            ErrorStartSaveTaskMessage,
            ErrorEmptyUserInputSaveTaskMessage,
            AskSaveTaskIdMessage,
            ShowLetEmptyRowForDefault,
            AskSaveTaskModifyNameMessage,
            AskSaveTaskModifyTargetFolderMessage,
            AskSaveTaskModifySourceFolderMessage,
            AskSaveTaskModifyType,
            ShowSaveTaskDetailsMessage,
            ShowSaveTaskNameMessage,
            ShowSaveTaskSourcePathMessage,
            ShowSaveTaskTargetPathMessage,
            ShowSaveTaskTypeMessage,
            ErrorSaveTaskNameDuplicateMessage
    }

    // Static class providing utility methods for message management
    public static class MessageExtensions
    {
        // Dictionary mapping EMessage enum values to their string representations
        private static readonly Dictionary<EMessage, string> MessageStrings = new Dictionary<EMessage, string>
        {
            //Based Cli Message
            { EMessage.ErrorMessage, "ErrorMessage" },
            { EMessage.ErrorStartScreenLoading, "ErrorStartScreenLoading" },
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
            { EMessage.ShowSaveTaskRegisterMessage, "ShowSaveTaskRegisterMessage" },
            { EMessage.ErrorSaveTaskNotFoundMessage, "ErrorSaveTaskNotFoundMessage" },
            { EMessage.SuccessStartSaveTaskMessage, "SuccessStartSaveTaskMessage" },
            { EMessage.SuccessModifySaveTaskMessage, "SuccessModifySaveTaskMessage" },
            { EMessage.SuccessSuppressSaveTaskMessage, "SuccessSuppressSaveTaskMessage" },
            { EMessage.ErrorModifySaveTaskMessage, "ErrorModifySaveTaskMessage" },
            { EMessage.ErrorSuppressSaveTaskMessage, "ErrorSuppressSaveTaskMessage" },
            { EMessage.ErrorStartEndIndexSaveTaskMessage, "ErrorStartEndIndexSaveTaskMessage" },
            { EMessage.ErrorStartSaveTaskMessage, "ErrorStartSaveTaskMessage" },
            { EMessage.ErrorEmptyUserInputSaveTaskMessage, "ErrorEmptyUserInputSaveTaskMessage" },
            { EMessage.AskSaveTaskIdMessage, "AskSaveTaskIdMessage" },
            { EMessage.ShowLetEmptyRowForDefault, "ShowLetEmptyRowForDefault" },
            { EMessage.AskSaveTaskModifyNameMessage, "AskSaveTaskModifyNameMessage" },
            { EMessage.AskSaveTaskModifyTargetFolderMessage, "AskSaveTaskModifyTargetFolderMessage" },
            { EMessage.AskSaveTaskModifySourceFolderMessage, "AskSaveTaskModifySourceFolderMessage" },
            { EMessage.AskSaveTaskModifyType, "AskSaveTaskModifyType" },
            { EMessage.ShowSaveTaskDetailsMessage, "ShowSaveTaskDetailsMessage" },
            { EMessage.ShowSaveTaskNameMessage, "ShowSaveTaskNameMessage" },
            { EMessage.ShowSaveTaskSourcePathMessage, "ShowSaveTaskSourcePathMessage" },
            { EMessage.ShowSaveTaskTargetPathMessage, "ShowSaveTaskTargetPathMessage" },
            { EMessage.ShowSaveTaskTypeMessage, "ShowSaveTaskTypeMessage" },
            { EMessage.ErrorSaveTaskNameDuplicateMessage, "ErrorSaveTaskNameDuplicateMessage" },
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
