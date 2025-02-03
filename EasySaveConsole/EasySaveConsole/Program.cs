using EasySaveConsole.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole.CLI;
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
            //création des vues 
            CliView cliView = new CliView();
            SaveTaskView saveTaskView = new SaveTaskView();
            //Création des modèles 
            LangagesManager langagesManager = new LangagesManager();
            MessagesManager messagesManager = new MessagesManager();
            SaveTaskManager saveTaskManager = new SaveTaskManager();
            // création des controllers
            LanguageController languageController = new LanguageController(langagesManager);
            SaveTaskController saveTaskController = new SaveTaskController(messagesManager, saveTaskView, saveTaskManager);
            CliController cliController = new CliController(messagesManager, cliView, saveTaskController, languageController);
            cliController.startCli();
        }
    }
}
