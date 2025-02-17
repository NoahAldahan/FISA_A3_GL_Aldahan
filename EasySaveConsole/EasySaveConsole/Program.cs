﻿using EasySaveConsole.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DotNetEnv;
using System.IO;
using EasySaveConsole.Utilities;
using EasySaveConsole.Controller;
using EasySaveConsole.View;
using Log;
using System.Text.Json;
using EasySaveConsole.Model.Log;

namespace EasySaveConsole
{
    // Main program entry point
    internal class Program
    {
        static void Main(string[] args)
        {
            // Load environment variables from the .env file
            Env.Load(@".env");

            // Create instances of views
            CliView cliView = new CliView();
            SaveTaskView saveTaskView = new SaveTaskView();
            LanguageView languageView = new LanguageView();
            LogView logView = new LogView();
            //Create Extension Instance
            MessageExtensions messageExtensions = new MessageExtensions();
            LanguageExtension languageExtensions = new LanguageExtension();
            LogSaveTaskTypeExtension logSaveTaskTypeExtension = new LogSaveTaskTypeExtension();

            // Create instances of models
            LanguageManager languageManager = new LanguageManager(languageExtensions);
            MessageManager messagesManager = new MessageManager(languageManager, messageExtensions);
            LogManager logManager = new LogManager(logSaveTaskTypeExtension);
            SaveTaskManager saveTaskManager = new SaveTaskManager(logManager);

            // Create instances of controllers
            LanguageController languageController = new LanguageController(messagesManager, languageView, languageManager);
            LogController logController = new LogController(messagesManager, logView, logManager);
            SaveTaskController saveTaskController = new SaveTaskController(messagesManager, saveTaskView, saveTaskManager, logController);
            CliController cliController = new CliController(messagesManager, cliView, saveTaskController, languageController);
            // Show the start screen
            cliController.showStart();

            // Start the CLI controller
            cliController.StartCli();

            // Uncommented debugging/testing code for directory pairs and logging

            // DirectoryPair directoryPair = new DirectoryPair("C:\\Users\\matte\\OneDrive\\Bureau\\src", "C:\\Users\\matte\\OneDrive\\Bureau\\target");
            // JsonLogManager jsonLogManager = new JsonLogManager();
            // SaveTask saveTask = new SaveTaskComplete(directoryPair, new LogDaily(jsonLogManager), new LogRealTime(jsonLogManager));
            // Console.WriteLine(saveTask.GetTotalFilesToCopy("C:\\Users\\matte\\OneDrive\\Bureau\\target"));
            // saveTask.Save();
            // jsonLogManager.CreateDailyJsonFile(DateTime.Now);
        }
    }
}
