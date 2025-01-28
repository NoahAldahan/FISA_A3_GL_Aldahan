using EasySaveConsole.Utilities;
using EasySaveConsole.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.CLI   
{
    public class CLI
    {
            
        // Définition de l'énumération
        enum CliState
        {   
            InProgress = 0,    // Par défaut, la numérotation commence à 0
            Stop = 1,
            Error = 2, 
            Waiting = 3,
        }
        CliState state;
        MessageVM messageViewModel;

        private void stopEvent()
        {
            
            state = CliState.Stop;
            messageViewModel.SelectedMessage = Messages.StopMessage;
            Console.WriteLine(messageViewModel.CurrentMessage);
        }

        public void CliApp()
        {
            state = CliState.Waiting;
            messageViewModel = new MessageVM();
            messageViewModel.SelectedMessage = Messages.InitMessage;
            Console.WriteLine(messageViewModel.CurrentMessage);

            while (state != CliState.Stop)
            {
                int userInput = int.Parse(Console.ReadLine());
                switch (userInput)
                {
                    case ((int)CliState.Stop):
                        stopEvent();
                        break;
                }
            }
        }
    }
}
