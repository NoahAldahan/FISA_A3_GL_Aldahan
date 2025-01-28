using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Utilities
{
        public enum Messages
        {
            ErrorTranslate,
            InitMessage,
            StopMessage,
            StartSaveTaskMessage,
            LanguageChangeSuccessMessage,
            CreateSaveTaskMessage
        }

        public static class MessagesExtensions
        {
            private static readonly Dictionary<Messages, string> dictionary = new Dictionary<Messages, string> {
            { Messages.InitMessage, "InitMessage" },
            { Messages.ErrorTranslate, "ErrorTranslate" },
            { Messages.StopMessage, "StopMessage" },
            { Messages.StartSaveTaskMessage, "StartSaveTaskMessage" },
            { Messages.LanguageChangeSuccessMessage, "LanguageChangeSuccessMessage" },
            { Messages.CreateSaveTaskMessage, "CreateSaveTaskMessage" }

        };

            private static readonly Dictionary<Messages, string> MessageStrings = dictionary;

            public static string GetValue(this Messages message)
            {
                if (MessageStrings.TryGetValue(message, out var value))
                {
                    return value;
                }
                throw new ArgumentException($"No string value defined for message: {message}");
            }
        }
}
