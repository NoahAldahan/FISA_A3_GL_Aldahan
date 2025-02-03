using EasySaveConsole.Model;
using EasySaveConsole.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using EasySaveConsole.Controller;
using Sprache;
using Microsoft.SqlServer.Server;

namespace EasySaveConsole.CLI   
{
    public class CliView
    {

        internal void ShowMessage(string message) 
        {
            Console.WriteLine(message);
        }

        internal string getUserInput(string input) 
        {
            Console.Write(input);
            string userInput = Console.ReadLine();
            return userInput;
        }

        public int GetOptionUserInput()
        {
            return int.Parse(Console.ReadLine());
        }
        public string GetUserInput() 
        {
            return Console.ReadLine();
        }

        public void showMessage(string message) 
        {
            Console.WriteLine(message);
        }

        internal string showQuestion(string question)
        {
            Console.Write(question);
            return Console.ReadLine();
        }
    }
}
