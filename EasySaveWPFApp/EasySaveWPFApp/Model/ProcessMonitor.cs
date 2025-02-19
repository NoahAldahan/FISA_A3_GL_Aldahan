using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace EasySaveConsole.Utilities
{
    public class BusinessSoftwareObserver
    {
        private readonly System.Timers.Timer timer;
        private const string businessSoftware = "cmd.exe"; // Logiciel métier fixe
        private bool isRunning;

        public event Action<bool> OnBusinessSoftwareStatusChanged;

        public BusinessSoftwareObserver()
        {
            timer = new System.Timers.Timer(1000); // Vérification toutes les secondes
            timer.Elapsed += CheckBusinessSoftware;
            timer.AutoReset = true;
            timer.Start();
        }

        private void CheckBusinessSoftware(object sender, ElapsedEventArgs e)
        {
            bool currentlyRunning = Process.GetProcessesByName("cmd").Any();

            if (currentlyRunning != isRunning)
            {
                isRunning = currentlyRunning;
                OnBusinessSoftwareStatusChanged?.Invoke(isRunning);
            }
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}

