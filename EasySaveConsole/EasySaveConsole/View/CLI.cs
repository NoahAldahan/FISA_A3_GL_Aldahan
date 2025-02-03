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

namespace EasySaveConsole.CLI   
{
    public class CLI
    {
        enum CliAction
        {   
            Stop = 0,
            Init = 1,
            ChangeDefaultLangage = 2,
            Langages = 3
        }

        CliAction state;
        MessageController messageController;

        private void StopEvent()
        {
            
            state = CliAction.Stop;
            Console.WriteLine(messageController.GetMessage(Messages.StopMessage));
        }

        private void ChangeDefaultLangage(string langage)
        {
            string msg = messageController.ChangeDefaultLangage(langage);
            Console.WriteLine(msg);
        }

        private void ShowOptions()
        {
            Console.WriteLine(messageController.GetMessage(Messages.InitMessage));
        }

        private void ShowLangages()
        {
            Console.WriteLine(messageController.GetMessage(Messages.LangagesMessage));
        }
        private void ShowAskLangage() 
        {
            Console.Write(messageController.GetMessage(Messages.AskLangageMessage));
        }
        private void InitCli()
        {
            ShowOptions();
        }

        public void CliApp()
        {
            messageController = new MessageController();
            state = CliAction.Init;
            InitCli();

            while (state != CliAction.Stop)
            {
                try
                {
                    int userInput = int.Parse(Console.ReadLine());
                    if (Enum.IsDefined(typeof(CliAction), userInput))
                    {
                        switch (userInput)
                        {
                            case ((int)CliAction.Stop):
                                StopEvent();
                                break;
                            case ((int)CliAction.Init):
                                ShowOptions();
                                break;
                            case ((int)CliAction.Langages):
                                ShowLangages();
                                break;
                            case ((int)CliAction.ChangeDefaultLangage):
                                ShowAskLangage();
                                string langageChoice = Console.ReadLine().ToString();
                                ChangeDefaultLangage(langageChoice);
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine(messageController.GetMessage(Messages.ErrorUserEntryOptionMessage));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(messageController.GetMessage(Messages.ErrorUserEntryStrMessage));
                }
            }
        }
    }
}
