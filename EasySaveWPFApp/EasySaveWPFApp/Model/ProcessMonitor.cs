using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace EasySaveConsole.Utilities
{
    public class ProcessMonitor
    {
        private readonly System.Timers.Timer timer;
        private const string businessSoftware = "cmd.exe"; // Logiciel métier fixe
        private bool isSoftwareRunning;

        // Événement déclenché lorsque l'état du logiciel change
        public event Action<bool> OnSoftwareStatusChanged;

        public ProcessMonitor()
        {
            timer = new System.Timers.Timer(1000); // Vérification toutes les secondes
            timer.Elapsed += CheckBusinessSoftware;
            isSoftwareRunning = false;
            timer.AutoReset = true;
            timer.Start();
        }

        private void CheckBusinessSoftware(object sender, ElapsedEventArgs e)
        {
            bool currentlyRunning = Process.GetProcessesByName("cmd").Any();

            if (currentlyRunning != isSoftwareRunning)
            {
                isSoftwareRunning = currentlyRunning;
                OnSoftwareStatusChanged?.Invoke(isSoftwareRunning); // Déclenche l'événement
            }
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}
