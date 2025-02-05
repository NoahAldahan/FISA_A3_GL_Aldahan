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
            SaveTaskManager saveTaskManager = new SaveTaskManager();
            // création des controllers
            LanguageController languageController = new LanguageController(messagesManager, languageView, languageManager);
            SaveTaskController saveTaskController = new SaveTaskController(messagesManager, saveTaskView, saveTaskManager);
            //idée faire un controller factory pour ne pas passer trop de dépendance à cliController
            CliController cliController = new CliController(messagesManager, cliView, saveTaskController, languageController);
            cliController.StartCli();
        }
    }
}