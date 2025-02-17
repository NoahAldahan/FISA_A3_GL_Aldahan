using EasySaveConsole.Model;
using EasySaveConsole.Model.Log;
using EasySaveConsole.Utilities;
using EasySaveConsole.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Controller
{
    // Enum defining the possible CLI language actions
    enum ECliLogAction
    {
        InitMenu = 0,    // Action to initialize the language menu
        ChangeLog = 1, // Action to change the language
        Quit = 2         // Action to quit the language menu
    }
    internal class LogController : BaseController
    {
        LogManager logManager;
        internal LogController(MessageManager messageManager, LogView view, LogManager logManager)
            : base(messageManager, view)
        {
            this.logManager = logManager;
            initCondition = (int)ECliLanguageAction.InitMenu; // Set the initial condition to the InitMenu action
            stopCondition = (int)ECliLanguageAction.Quit; // Set the stop condition to the Quit action
            InitDictAction();
        }

        protected override void InitDictAction()
        {
            dictActions.Add((int)ECliLogAction.InitMenu, () => { ShowMessage(EMessage.MenuLogSaveTaskTypeMessage); }); // Add action to show the language menu
            dictActions.Add((int)ECliLogAction.ChangeLog, () => { SetDefaultLogSaveTaskType(); }); // Add action to change the language
            dictActions.Add((int)ECliLogAction.Quit, () => { ShowMessage(EMessage.StopMessage); }); // Add action to quit the language menu
        }

        // Method to set the default language
        internal void SetDefaultLogSaveTaskType()
        {
            ShowMessage(EMessage.LogSaveTaskTypeListMessage); // Show the list of available languages
            string strLogSaveTaskType = ShowQuestion(EMessage.AskLogTaskTypeMessage); // Ask the user to select a language
            EMessage msg = logManager.SetDefaultLog(strLogSaveTaskType); // Set the selected language as the default
            ShowMessagePause(msg); // Show a message and wait for user confirmation
        }
    }
}
