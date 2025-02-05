﻿using EasySaveConsole.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model
{
        public enum EMessage
        {
            ErrorTranslate,
            MenuMessage,
            LanguagesListMessage,
            StopMessage,
            StartSaveTaskMessage,
            LanguageChangeSuccessMessage,
            CreateSaveTaskMessage,
            DefaultLanguageInitSuccessMessage,
            DefaultLanguageInitErrorMessage,
            DefaultLanguageChangedSuccessMessage,
            DefaultLanguageChangedErrorMessage,
            AskLanguageMessage,
            ErrorUserEntryStrMessage,
            ErrorUserEntryOptionMessage,
            MenuSaveTaskMessage,
            MenuLanguageMessage,
            PressKeyToContinue
        }

        public static class MessageExtensions
        {
            private static readonly Dictionary<EMessage, string> MessageStrings = new Dictionary<EMessage, string> {
            { EMessage.MenuMessage, "MenuMessage" },
            { EMessage.ErrorTranslate, "ErrorTranslate" },
            { EMessage.StopMessage, "StopMessage" },
            { EMessage.LanguagesListMessage, "LanguagesListMessage" },
            { EMessage.StartSaveTaskMessage, "StartSaveTaskMessage" },
            { EMessage.LanguageChangeSuccessMessage, "LanguageChangeSuccessMessage" },
            { EMessage.CreateSaveTaskMessage, "CreateSaveTaskMessage" },
            { EMessage.DefaultLanguageInitSuccessMessage, "DefaultLanguageInitSuccessMessage" },
            { EMessage.DefaultLanguageInitErrorMessage, "DefaultLanguageInitErrorMessage" },
            { EMessage.DefaultLanguageChangedSuccessMessage, "DefaultLanguageChangedSuccessMessage" },
            { EMessage.DefaultLanguageChangedErrorMessage, "DefaultLanguageChangedErrorMessage" },
            { EMessage.AskLanguageMessage, "AskLanguageMessage" },
            { EMessage.ErrorUserEntryStrMessage, "ErrorUserEntryStrMessage" },
            { EMessage.ErrorUserEntryOptionMessage, "ErrorUserEntryOptionMessage" },
            { EMessage.MenuSaveTaskMessage, "MenuSaveTaskMessage" },
            { EMessage.MenuLanguageMessage, "MenuLanguageMessage" },
            { EMessage.PressKeyToContinue, "PressKeyToContinue" }
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
            JsonManager jsonManager;
            public MessageManager(LanguageManager languageManager, JsonManager jsonManager)
            {
                this.languageManager = languageManager;
                this.jsonManager = jsonManager;
            }
            public string GetMessageTranslate(EMessage message)
            {   
                return jsonManager.GetMessage(message.GetValue(), languageManager.defaultLanguage);
            }
        }
}
