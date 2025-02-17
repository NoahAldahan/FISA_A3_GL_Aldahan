using EasySaveConsole.Model;
using EasySaveConsole.View;
using System;

namespace EasySaveConsole.Controller
{
    // Enum defining the possible CLI actions
    enum ECliAction
    {
        InitMenu = 0,    // Action to initialize the main menu
        LanguageMenu = 1, // Action to display the language menu
        SaveMenu = 2,     // Action to display the save menu
        Stop = 3        // Action to stop the CLI
    }

    // Controller class for managing the command-line interface (CLI)
    internal class CliController : BaseController
    {
        // Controller for managing save tasks
        private SaveTaskController saveTaskController;

        // Controller for managing language settings
        private LanguageController languageController;

        private LogController logController;

        // Constructor for the CLI controller
        internal CliController(MessageManager messagesManager, CliView view, SaveTaskController saveTaskController, LanguageController languageController)
            : base(messagesManager, view)
        {
            this.saveTaskController = saveTaskController;
            this.languageController = languageController;
            stopCondition = (int)ECliAction.Stop; // Set the stop condition to the Stop action
            initCondition = (int)ECliAction.InitMenu; // Set the initial condition to the InitMenu action
            InitDictAction(); // Initialize the dictionary of actions
        }

        // Override the base method to initialize the dictionary of actions
        protected override void InitDictAction()
        {
            dictActions.Add((int)ECliAction.InitMenu, () => { ShowMessage(EMessage.MenuMessage); }); // Add action to show the main menu
            dictActions.Add((int)ECliAction.Stop, () => { ExitCli(); }); // Add action to exit the CLI
            dictActions.Add((int)ECliAction.LanguageMenu, () => { languageController.StartCli(); }); // Add action to start the language menu
            dictActions.Add((int)ECliAction.SaveMenu, () => { saveTaskController.StartCli(); }); // Add action to start the save menu
        }

        internal void showStart()
        {
            try
            {
                ((CliView)view).ShowStartScreen();
            }
            catch (System.Exception e)
            {
                ShowMessage(EMessage.ErrorStartScreenLoading);
            }
        }

        // Method to exit the CLI
        private void ExitCli()
        {
            ShowMessage(EMessage.StopMessage); // Show the stop message
            saveTaskController.saveTaskManager.SerializeSaveTasks(); // Serialize the save tasks before exiting
        }
    }
}

