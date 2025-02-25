using System.Collections.Generic;
using System.Windows;
using EasySaveWPFApp.Model;

namespace EasySaveWPFApp
{
    public partial class SaveTaskStoppedWindow : Window
    {
        List<SaveTask> stoppedTasks;
        public SaveTaskStoppedWindow(List<SaveTask> stoppedTasks)
        {
            InitializeComponent();
            this.stoppedTasks = stoppedTasks; // Injection directe des données
            DataContext = this;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}

