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
            initCondition = -1;
            stopCondition = -2;
        }

        protected abstract void InitDictAction();

        protected void ShowMessage(EMessage msg)
        {
            string translatedMessage = messagesManager.GetMessageTranslate(msg);
            view.ShowMessage(translatedMessage);
        }

        protected string ShowQuestion(EMessage msg)
        {
            string translatedMessage = messagesManager.GetMessageTranslate(msg);
            return view.ShowQuestion(translatedMessage);
        }

        internal void StartCli()
        {
            int action = initCondition;
            while(action != stopCondition)
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
