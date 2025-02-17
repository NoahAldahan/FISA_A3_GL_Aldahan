using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveWeb.View
{
    // Abstract base class for handling console interactions
    public abstract class BaseView
    {
        // Displays a message in the console
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        // Prompts the user with a message and retrieves input as a string
        public string GetUserInput(string input)
        {
            Console.Write(input);
            string userInput = Console.ReadLine();
            return userInput;
        }

        // Retrieves user input as an integer (may throw an exception if invalid)
        public int GetOptionUserInput()
        {
            return int.Parse(Console.ReadLine());
        }

        // Retrieves user input as a string
        public string GetUserInput()
        {
            return Console.ReadLine();
        }

        // Displays a question and returns the user's response
        public string ShowQuestion(string question)
        {
            Console.Write(question);
            return Console.ReadLine();
        }

        // Clears the console screen
        public void Clear()
        {
            Console.Clear();
        }
    }
}

