using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Model.Log
{
    internal class LogDaily : ILogObserver
    {
        public void Notify(Dictionary<string, object> message)
        {
            foreach (var item in message)
            {
                Console.WriteLine(item.ToString());
            }
        }

    }
}
