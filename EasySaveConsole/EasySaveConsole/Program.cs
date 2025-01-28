using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole.CLI;
using DotNetEnv;
using System.IO;
using EasySaveConsole.Utilities;


namespace EasySaveConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Charger les variables d'environnement depuis le fichier .env
            Env.Load(@".env");
            CLI.CLI cLI = new CLI.CLI();
            cLI.CliApp();
        }
    }
}
