using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.CLI   
{
    public class CLI
    {

        // Définition de l'énumération
        enum taskState
        {
            InProgress = 0,    // Par défaut, la numérotation commence à 0
            Stop = 1,
            Error = 2, 
            Waiting = 3,
        }

        public void test()
        {
            taskState State = taskState.Waiting;
            while(State != taskState.Stop)
            {
                Console.WriteLine("Choose your options : \n 1 : stop ");
                int userInput = int.Parse(Console.ReadLine());
                if (userInput.Equals(userInput))
                {
                    Console.WriteLine("Arrêt du programme de sauvegarde");
                    State = taskState.Stop;
                }
            }

        }
    }

}
