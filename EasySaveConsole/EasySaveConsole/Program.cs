using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole.CLI;


namespace EasySaveConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CLI.CLI cLI = new CLI.CLI();
            cLI.test();
        }
    }
}
