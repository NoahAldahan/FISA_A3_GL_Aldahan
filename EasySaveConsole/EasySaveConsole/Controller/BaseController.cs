using EasySaveConsole.Model;
using EasySaveConsole.View;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace EasySaveConsole.Controller
{
    internal abstract class BaseController
    {
        protected MessageManager messagesManager;
        protected BaseView view;
        protected Dictionary<int, Action> dictActions;
        protected int initCondition;
        protected int stopCondition;

        protected BaseController(MessageManager messagesManager, BaseView view)
        {
            this.messagesManager = messagesManager;
            this.view = view;
            dictActions = new Dictionary<int, Action>();
        }

        protected abstract void InitDictAction();

        protected void ShowMessage(EMessage msg)
        {
            string translatedMessage = messagesManager.GetMessageTranslate(msg);
            view.ShowMessage(translatedMessage);
        }
        protected void ShowMessage(string msg)
        {
            view.ShowMessage(msg);
        }


        protected string ShowQuestion(EMessage msg)
        {
            string translatedMessage = messagesManager.GetMessageTranslate(msg);
            return view.ShowQuestion(translatedMessage);
        }

        protected void ShowMessagePause(EMessage msg)
        {
            ShowMessage(msg);
            ShowQuestion(EMessage.PressKeyToContinue);
        }

        internal void StartCli()
        {
            int action = initCondition;
            while (action != stopCondition)
            {
                try
                {
                    //on vérifie si la initCondition est bien spécifié dans le dictionnaire
                    if (dictActions.ContainsKey(initCondition))
                    {
                        view.Clear();
                        dictActions[initCondition]();//action d'init (affiche le menu)
                        action = this.view.GetOptionUserInput();
                        if (dictActions.ContainsKey(action))
                        {
                            dictActions[action]();
                        }
                        else
                        {
                            ShowMessage(EMessage.ErrorUserEntryOptionMessage);
                        }
                    }
                    else
                    {
                        //afficher error message
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage(EMessage.ErrorUserEntryStrMessage);
                }
            }
        }
    }
}
