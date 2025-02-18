using EasySaveWPFApp.Model;
using EasySaveWPFApp.View;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EasySaveWPFApp.Controller
{
    // Abstract base class for controllers
    internal abstract class BaseController
    {
        // Message manager for translating and displaying messages
        protected MessageManager messagesManager;

        // View associated with the controller
        protected BaseView view;

        // Dictionary mapping integer keys to actions
        protected Dictionary<int, Action> dictActions;

        // Initial condition to start the CLI
        protected int initCondition;

        // Stop condition for the CLI
        protected int stopCondition;

        // Constructor for the base controller class
        protected BaseController(MessageManager messagesManager, BaseView view)
        {
            this.messagesManager = messagesManager;
            this.view = view;
            dictActions = new Dictionary<int, Action>();
        }

        // Abstract method to initialize the dictionary of actions
        protected abstract void InitDictAction();

        // Method to display a translated message
        protected void ShowMessage(EMessage msg)
        {
            string translatedMessage = messagesManager.GetMessageTranslate(msg);
            view.ShowMessage(translatedMessage);
        }

        // Method to display a raw (untranslated) message
        protected void ShowMessage(string msg)
        {
            view.ShowMessage(msg);
        }

        protected string ShowQuestion(string msg)
        {
            return view.ShowQuestion(msg);
        }

        // Method to ask a question and get a response from the user
        protected string ShowQuestion(EMessage msg)
        {
            string translatedMessage = messagesManager.GetMessageTranslate(msg);
            return view.ShowQuestion(translatedMessage);
        }

        // Method to display a message and wait for user confirmation
        protected void ShowMessagePause(EMessage msg)
        {
            ShowMessage(msg);
            ShowQuestion(EMessage.PressKeyToContinue);
        }

        protected void ShowMessagePause(string msg)
        {
            ShowMessage(msg);
            ShowQuestion(EMessage.PressKeyToContinue);
        }

        // Main method to start the command-line interface (CLI)
        internal void StartCli()
        {
            int action = initCondition;
            while  (action != stopCondition)
            {
                try
                {
                    // Check if the initial condition is present in the dictionary of actions
                    if (dictActions.ContainsKey(initCondition))
                    {
                        view.Clear(); // Clear the view
                        dictActions[initCondition](); // Execute the initialization action (display the menu)
                        action = this.view.GetOptionUserInput(); // Get the option chosen by the user
                        if (dictActions.ContainsKey(action))
                        {
                            dictActions[action](); // Execute the action associated with the chosen option
                        }
                        else
                        {
                            ShowMessage(EMessage.ErrorUserEntryOptionMessage); // Display an error message if the option is invalid
                        }
                    }
                    else
                    {
                        // Display an error message if the initial condition is invalid
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage(EMessage.ErrorUserEntryStrMessage); // Display an error message in case of an exception
                }
            }
        }
    }
}


