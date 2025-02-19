using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveWPFApp.Model
{
    internal interface ESaveTaskObserver
    {
        void NotifySoftwareRunning(bool isSoftwareRunning);
    }
}
