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
            //Création des modèles 
            MessagesManager messagesManager = new MessagesManager();
            // création des controllers
            MessageController messageController = new MessageController(messagesManager);
            CliController cliController = new CliController(messageController, cliView);
            cliController.startCli();

           
        }
    }
}
