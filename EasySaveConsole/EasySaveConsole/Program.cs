using EasySaveConsole.Model;
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


namespace EasySaveConsole
{
    internal class Program
    {

        static void Main(string[] args)
        {
            // Charger les variables d'environnement depuis le fichier .env
            Env.Load(@".env");
            //create utilities
            JsonManager jsonManager = new JsonManager();
            //création des vues 
            CliView cliView = new CliView();
            SaveTaskView saveTaskView = new SaveTaskView();
            LanguageView languageView = new LanguageView();
            //Création des modèles 
            LanguageManager languageManager = new LanguageManager();
            MessageManager messagesManager = new MessageManager(languageManager, jsonManager);
            SaveTaskManager saveTaskManager = new SaveTaskManager(jsonManager);
            // création des controllers
            LanguageController languageController = new LanguageController(messagesManager, languageView, languageManager);
            SaveTaskController saveTaskController = new SaveTaskController(messagesManager, saveTaskView, saveTaskManager);
            CliController cliController = new CliController(messagesManager, cliView, saveTaskController, languageController);
            cliController.StartCli();
            //C:\\Users\\Travail\\Desktop\\Source
            //C:\\Users\\Travail\\Desktop\\Target
            DirectoryPair directoryPair = new DirectoryPair("C:\\Users\\matte\\OneDrive\\Bureau\\src", "C:\\Users\\matte\\OneDrive\\Bureau\\target");
            //Console.WriteLine(saveTask.GetTotalFilesToCopy("C:\\Users\\matte\\OneDrive\\Bureau\\target"));
            //JsonLogManager jsonLogManager = new JsonLogManager();
            //jsonLogManager.CreateDailyJsonFile(DateTime.Now);
        }
    }
}