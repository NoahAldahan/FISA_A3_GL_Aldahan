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
            SuccessCreateSaveTaskMessage,
            SaveTaskTypeCompleteName,
            SaveTaskTypeDifferentialName,
            ErrorStartSaveTaskPathListMessage,

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
            ErrorSaveTaskNameDuplicateMessage,
            //LogSaveTaskType
            LogSaveTaskTypeChangeErrorMessage,
            LogSaveTaskTypeChangeSuccessMessage,
            MenuLogSaveTaskTypeMessage,
            DefaultLogTaskTypeInitSuccessMessage,
            DefaultLogTaskTypeInitErrorMessage,
            DefaultLogTaskTypeChangedSuccessMessage,
            DefaultLogTaskTypeChangedErrorMessage,
            LogSaveTaskTypeListMessage,
            AskLogTaskTypeMessage,
    }

    // Static class providing utility methods for message management
    internal class MessageExtensions : EnumExtension<EMessage>
    {
        internal MessageExtensions()
        {
            enumStrings.Add(EMessage.ErrorMessage, "ErrorMessage");
            enumStrings.Add(EMessage.ErrorStartScreenLoading, "ErrorStartScreenLoading");
            enumStrings.Add(EMessage.MenuMessage, "MenuMessage");
            enumStrings.Add(EMessage.StopMessage, "StopMessage");
            enumStrings.Add(EMessage.PressKeyToContinue, "PressKeyToContinue");
            enumStrings.Add(EMessage.ErrorUserEntryStrMessage, "ErrorUserEntryStrMessage");
            enumStrings.Add(EMessage.ErrorUserEntryOptionMessage, "ErrorUserEntryOptionMessage");

            // Language-related Messages
            enumStrings.Add(EMessage.LanguagesListMessage, "LanguagesListMessage");
            enumStrings.Add(EMessage.LanguageChangeSuccessMessage, "LanguageChangeSuccessMessage");
            enumStrings.Add(EMessage.DefaultLanguageInitSuccessMessage, "DefaultLanguageInitSuccessMessage");
            enumStrings.Add(EMessage.DefaultLanguageInitErrorMessage, "DefaultLanguageInitErrorMessage");
            enumStrings.Add(EMessage.DefaultLanguageChangedSuccessMessage, "DefaultLanguageChangedSuccessMessage");
            enumStrings.Add(EMessage.DefaultLanguageChangedErrorMessage, "DefaultLanguageChangedErrorMessage");
            enumStrings.Add(EMessage.AskLanguageMessage, "AskLanguageMessage");
            enumStrings.Add(EMessage.MenuLanguageMessage, "MenuLanguageMessage");

            // Save Task-related Messages
            enumStrings.Add(EMessage.StartSaveTaskMessage, "StartSaveTaskMessage");
            enumStrings.Add(EMessage.CreateSaveTaskMessage, "CreateSaveTaskMessage");
            enumStrings.Add(EMessage.MenuSaveTaskMessage, "MenuSaveTaskMessage");
            enumStrings.Add(EMessage.AskSaveTaskNameMessage, "AskSaveTaskNameMessage");
            enumStrings.Add(EMessage.AskSaveTaskTargetFolderMessage, "AskSaveTaskTargetFolderMessage");
            enumStrings.Add(EMessage.AskSaveTaskSourceFolderMessage, "AskSaveTaskSourceFolderMessage");
            enumStrings.Add(EMessage.AskSaveTaskType, "AskSaveTaskType");
            enumStrings.Add(EMessage.ErrorSaveTaskPathMessage, "ErrorSaveTaskPathMessage");
            enumStrings.Add(EMessage.ErrorSaveTaskTypeMessage, "ErrorSaveTaskTypeMessage");
            enumStrings.Add(EMessage.SaveTaskAddSuccessMessage, "SaveTaskAddSuccessMessage");
            enumStrings.Add(EMessage.ShowSaveTaskRegisterMessage, "ShowSaveTaskRegisterMessage");
            enumStrings.Add(EMessage.ErrorSaveTaskNotFoundMessage, "ErrorSaveTaskNotFoundMessage");
            enumStrings.Add(EMessage.SuccessStartSaveTaskMessage, "SuccessStartSaveTaskMessage");
            enumStrings.Add(EMessage.SuccessModifySaveTaskMessage, "SuccessModifySaveTaskMessage");
            enumStrings.Add(EMessage.SuccessSuppressSaveTaskMessage, "SuccessSuppressSaveTaskMessage");
            enumStrings.Add(EMessage.ErrorModifySaveTaskMessage, "ErrorModifySaveTaskMessage");
            enumStrings.Add(EMessage.ErrorSuppressSaveTaskMessage, "ErrorSuppressSaveTaskMessage");
            enumStrings.Add(EMessage.ErrorStartEndIndexSaveTaskMessage, "ErrorStartEndIndexSaveTaskMessage");
            enumStrings.Add(EMessage.ErrorStartSaveTaskMessage, "ErrorStartSaveTaskMessage");
            enumStrings.Add(EMessage.ErrorEmptyUserInputSaveTaskMessage, "ErrorEmptyUserInputSaveTaskMessage");
            enumStrings.Add(EMessage.AskSaveTaskIdMessage, "AskSaveTaskIdMessage");
            enumStrings.Add(EMessage.ShowLetEmptyRowForDefault, "ShowLetEmptyRowForDefault");
            enumStrings.Add(EMessage.AskSaveTaskModifyNameMessage, "AskSaveTaskModifyNameMessage");
            enumStrings.Add(EMessage.AskSaveTaskModifyTargetFolderMessage, "AskSaveTaskModifyTargetFolderMessage");
            enumStrings.Add(EMessage.AskSaveTaskModifySourceFolderMessage, "AskSaveTaskModifySourceFolderMessage");
            enumStrings.Add(EMessage.AskSaveTaskModifyType, "AskSaveTaskModifyType");
            enumStrings.Add(EMessage.ShowSaveTaskDetailsMessage, "ShowSaveTaskDetailsMessage");
            enumStrings.Add(EMessage.ShowSaveTaskNameMessage, "ShowSaveTaskNameMessage");
            enumStrings.Add(EMessage.ShowSaveTaskSourcePathMessage, "ShowSaveTaskSourcePathMessage");
            enumStrings.Add(EMessage.ShowSaveTaskTargetPathMessage, "ShowSaveTaskTargetPathMessage");
            enumStrings.Add(EMessage.ShowSaveTaskTypeMessage, "ShowSaveTaskTypeMessage");
            enumStrings.Add(EMessage.ErrorSaveTaskNameDuplicateMessage, "ErrorSaveTaskNameDuplicateMessage");

            //LogSaveTaskTypes
            enumStrings.Add(EMessage.LogSaveTaskTypeChangeSuccessMessage, "LogSaveTaskTypeChangeSuccessMessage");
            enumStrings.Add(EMessage.LogSaveTaskTypeChangeErrorMessage, "LogSaveTaskTypeChangeErrorMessage");
            enumStrings.Add(EMessage.MenuLogSaveTaskTypeMessage, "MenuLogSaveTaskTypeMessage");
            enumStrings.Add(EMessage.DefaultLogTaskTypeInitSuccessMessage, "DefaultLogTaskTypeInitSuccessMessage");
            enumStrings.Add(EMessage.DefaultLogTaskTypeInitErrorMessage, "DefaultLogTaskTypeInitErrorMessage");
            enumStrings.Add(EMessage.DefaultLogTaskTypeChangedSuccessMessage, "DefaultLogTaskTypeChangedSuccessMessage");
            enumStrings.Add(EMessage.DefaultLogTaskTypeChangedErrorMessage, "DefaultLogTaskTypeChangedErrorMessage");
            enumStrings.Add(EMessage.LogSaveTaskTypeListMessage, "LogSaveTaskTypeListMessage");
            enumStrings.Add(EMessage.AskLogTaskTypeMessage, "AskLogTaskTypeMessage");
            enumStrings.Add(EMessage.SuccessCreateSaveTaskMessage, "SuccessCreateSaveTaskMessage");
            enumStrings.Add(EMessage.SaveTaskTypeCompleteName, "SaveTaskTypeCompleteName");
            enumStrings.Add(EMessage.SaveTaskTypeDifferentialName, "SaveTaskTypeDifferentialName");
            enumStrings.Add(EMessage.ErrorStartSaveTaskPathListMessage, "ErrorStartSaveTaskPathListMessage");
        }
    }

        internal class MessageManager
        {
            LanguageManager languageManager;
            MessageExtensions messageExtensions;
            public MessageManager(LanguageManager languageManager, MessageExtensions messageExtensions)
            {
                this.languageManager = languageManager;
                this.messageExtensions = messageExtensions;
            }
            public string GetMessageTranslate(EMessage message) 
            {   
                return JsonManager.GetMessage(messageExtensions.GetValue(message), languageManager.getStrDefaultLanguage());
            }
        }
}
