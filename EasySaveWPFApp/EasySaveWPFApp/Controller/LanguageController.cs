using EasySaveWPFApp.Model;
using EasySaveWPFApp.Utilities;
using EasySaveWPFApp.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasySaveWPFApp.Controller
{
    // Enum defining the possible CLI language actions
    enum ECliLanguageAction
    {
        InitMenu = 0,    // Action to initialize the language menu
        ChangeLanguage = 1, // Action to change the language
        Quit = 2         // Action to quit the language menu
    }

    // Controller class for managing language settings in the CLI
    internal class LanguageController : BaseController
    {
        // Manager for handling language settings
        LanguageManager langaguesManager;

        // Constructor for the LanguageController class
        internal LanguageController(MessageManager messageManager, LanguageView view, LanguageManager langaguesManager)
            : base(messageManager, view)
        {
            this.langaguesManager = langaguesManager;
            initCondition = (int)ECliLanguageAction.InitMenu; // Set the initial condition to the InitMenu action
            stopCondition = (int)ECliLanguageAction.Quit; // Set the stop condition to the Quit action
            InitDictAction(); // Initialize the dictionary of actions
        }

        // Override the base method to initialize the dictionary of actions
        protected override void InitDictAction()
        {
            dictActions.Add((int)ECliLanguageAction.InitMenu, () => { ShowMessage(EMessage.MenuLanguageMessage); }); // Add action to show the language menu
            dictActions.Add((int)ECliLanguageAction.ChangeLanguage, () => { SetDefaultLanguage(); }); // Add action to change the language
            dictActions.Add((int)ECliLanguageAction.Quit, () => { ShowMessage(EMessage.StopMessage); }); // Add action to quit the language menu
        }

        // Method to set the default language
        internal void SetDefaultLanguage()
        {
            ShowMessage(EMessage.LanguagesListMessage); // Show the list of available languages
            string strLanguage = ShowQuestion(EMessage.AskLanguageMessage); // Ask the user to select a language
            EMessage msg = langaguesManager.SetDefaultLanguage(strLanguage); // Set the selected language as the default
            ShowMessagePause(msg); // Show a message and wait for user confirmation
        }
    }
}

