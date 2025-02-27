using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;

namespace EasySaveWPFApp.Model
{
    public class ProcessMonitor
    {
        private readonly System.Timers.Timer timer;
        private const string businessSoftware = "chrome"; // Logiciel métier fixe. DO NOT ADD .exe
        private bool isSoftwareRunning;

        // Événement déclenché lorsque l'état du logiciel change
        public event Action<bool> OnSoftwareStatusChanged;

        public ProcessMonitor()
        {
            timer = new System.Timers.Timer(1000); // Vérification toutes les secondes
            timer.Elapsed += async (sender, e) =>
            {
                await CheckBusinessSoftware();
            };
            isSoftwareRunning = false;
            timer.AutoReset = true;
            timer.Start();
        }

        private async Task CheckBusinessSoftware()
        {
            bool currentlyRunning = Process.GetProcessesByName(businessSoftware).Any();

            if (currentlyRunning != isSoftwareRunning)
            {
                Trace.WriteLine("Software status changed: "+currentlyRunning.ToString());
                isSoftwareRunning = currentlyRunning;
                OnSoftwareStatusChanged?.Invoke(isSoftwareRunning);
            }
        }

        public void Stop()
        {
            timer.Stop();
        }
    }
}
