using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.View
{
    public abstract class BaseView
    {
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }   

        public string GetUserInput(string input)
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

        public string ShowQuestion(string question)
        {
            Console.Write(question);
            return Console.ReadLine();
        }
    }
}
