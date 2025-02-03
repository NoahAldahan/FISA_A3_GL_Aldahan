using EasySaveConsole.CLI;
using EasySaveConsole.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Controller
{
    internal class CliController
    {
        MessageController messageController;
        CliView cliView;
        CliAction state;
        public CliController(MessageController messageControllerArg, CliView cliViewArg) 
        {
            messageController = messageControllerArg;
            cliView = cliViewArg;
            state = new CliAction();
        }

       internal void showMessage(Messages msg)
       {
            string strMsg = messageController.GetMessage(msg);
            cliView.showMessage(strMsg);
       }

        internal void startCli()
        {
            state = CliAction.Init;
            showMessage(Messages.InitMessage);
            while (state != CliAction.Stop)
            {
                try
                {
                    int userInput = cliView.GetOptionUserInput();
                    if (Enum.IsDefined(typeof(CliAction), userInput))
                    {
                        switch (userInput)
                        {
                            case ((int)CliAction.Stop):
                                showMessage(Messages.StopMessage);
                                break;
                            case ((int)CliAction.Init):
                                showMessage(Messages.InitMessage);
                                break;
                            case ((int)CliAction.Langages):
                                showMessage(Messages.LangagesMessage);
                                break;
                            case ((int)CliAction.ChangeDefaultLangage):
                                string msg = messageController.GetMessage(Messages.AskLangageMessage);
                                string langageChoice = cliView.showQuestion(msg);
                                Messages result = messageController.SetDefaultLangage(langageChoice);
                                showMessage(result);
                                break;
                        }
                    }
                    else
                    {
                        showMessage(Messages.ErrorUserEntryOptionMessage);
                    }
                }
                catch (Exception ex)
                {
                    showMessage(Messages.ErrorUserEntryStrMessage);
                }
            }
        }
    }
}
