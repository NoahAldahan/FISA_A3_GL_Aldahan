using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model
{
    enum ECliAction
    {   
        Stop = 0,
        Init = 1,
        ChangeDefaultLanguage = 2,
        Languages = 3,
        SaveMenu = 4,
    }
    enum ECliSaveTaskAction
    {
        Init = 0,
        StartTasks = 1,
        CreateTask = 2,
        ModifyTask = 3,
        DeleteTask = 4,
        Help = 5,
        Quit = 6
    }
}
