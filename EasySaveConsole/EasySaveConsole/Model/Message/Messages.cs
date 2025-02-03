using EasySaveConsole.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model
{
        public enum Messages
        {
            ErrorTranslate,
            InitMessage,
            LangagesMessage,
            StopMessage,
            StartSaveTaskMessage,
            LanguageChangeSuccessMessage,
            CreateSaveTaskMessage,
            DefaultLanguageInitSuccessMessage,
            DefaultLanguageInitErrorMessage,
            DefaultLanguageChangedSuccessMessage,
            DefaultLanguageChangedErrorMessage,
            AskLangageMessage,
            ErrorUserEntryStrMessage,
            ErrorUserEntryOptionMessage,
            SaveTaskMenuMessage
        }

        public static class MessagesExtensions
        {
            private static readonly Dictionary<Messages, string> MessageStrings = new Dictionary<Messages, string> {
            { Messages.InitMessage, "InitMessage" },
            { Messages.ErrorTranslate, "ErrorTranslate" },
            { Messages.StopMessage, "StopMessage" },
            { Messages.LangagesMessage, "LangagesMessage" },
            { Messages.StartSaveTaskMessage, "StartSaveTaskMessage" },
            { Messages.LanguageChangeSuccessMessage, "LanguageChangeSuccessMessage" },
            { Messages.CreateSaveTaskMessage, "CreateSaveTaskMessage" },
            { Messages.DefaultLanguageInitSuccessMessage, "DefaultLanguageInitSuccessMessage" },
            { Messages.DefaultLanguageInitErrorMessage, "DefaultLanguageInitErrorMessage" },
            { Messages.DefaultLanguageChangedSuccessMessage, "DefaultLanguageChangedSuccessMessage" },
            { Messages.DefaultLanguageChangedErrorMessage, "DefaultLanguageChangedErrorMessage" },
            { Messages.AskLangageMessage, "AskLangageMessage" },
            { Messages.ErrorUserEntryStrMessage, "ErrorUserEntryStrMessage" },
            { Messages.ErrorUserEntryOptionMessage, "ErrorUserEntryOptionMessage" },
            { Messages.SaveTaskMenuMessage, "SaveTaskMenuMessage" }

        };

            public static string GetValue(this Messages message)
            {
                if (MessageStrings.TryGetValue(message, out var value))
                {
                    return value;
                }
                throw new ArgumentException($"No string value defined for message: {message}");
            }

            public static Messages GetMessages(string message)
            {
                foreach (var pair in MessageStrings)
                {
                    if (pair.Value == message)
                    {
                        return pair.Key;
                    }
                }
                throw new ArgumentException($"No Messages enum value found for message: {message}");
            }
    }
        internal class MessagesManager
        {
            LangagesManager langagesManager;
            JsonManager jsonManager;
            public MessagesManager()
            {
                langagesManager = new LangagesManager();
                jsonManager = new JsonManager();
            }
            public string GetMessageTranslate(Messages message)
            {   
                return jsonManager.GetMessage(message, langagesManager.defaultLangage);
            }
        }
}
