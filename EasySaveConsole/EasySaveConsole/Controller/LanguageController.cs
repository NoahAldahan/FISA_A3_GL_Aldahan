using EasySaveConsole.Model;
using EasySaveConsole.Utilities;
using EasySaveConsole.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Controller
{
    enum ECliLanguageAction
    {
        InitMenu = 0,
        ChangeLanguage = 1,
        Quit = 2
    }
    internal class LanguageController : BaseController
    {
        LanguageManager langaguesManager;
        internal LanguageController(MessageManager messageManager, LanguageView view, LanguageManager langaguesManager) : base(messageManager, view) 
        {
            this.langaguesManager = langaguesManager;
            initCondition = (int)ECliLanguageAction.InitMenu;
            stopCondition = (int)ECliLanguageAction.Quit;
            InitDictAction();   
        }

        override protected void InitDictAction()
        {
            dictActions.Add((int)ECliLanguageAction.InitMenu, () => { ShowMessage(EMessage.MenuLanguageMessage); });
            dictActions.Add((int)ECliLanguageAction.ChangeLanguage, () => { SetDefaultLanguage(); });
            dictActions.Add((int)ECliLanguageAction.Quit, () => { ShowMessage(EMessage.StopMessage); });
        }

        internal void SetDefaultLanguage()
        {
            ShowMessage(EMessage.LanguagesListMessage);
            string strLanguage = ShowQuestion(EMessage.AskLanguageMessage);
            EMessage msg = langaguesManager.SetDefaultLanguage(strLanguage);
            ShowMessage(msg);
            ShowQuestion(EMessage.PressKeyToContinue);
        }
    }
}
