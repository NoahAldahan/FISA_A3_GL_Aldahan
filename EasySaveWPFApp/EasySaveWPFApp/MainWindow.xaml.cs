using EasySaveWPFApp.Model;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DotNetEnv;
using EasySaveWPFApp.ViewModel;
using EasySaveWPFApp.Api;
using EasySaveWPFApp.Utilities;
using System.Collections.ObjectModel;

namespace EasySaveWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        public SaveTaskViewModel saveTaskViewModel;
        SaveTaskWindow saveTaskWindow;
        SaveTaskManager saveTaskManager;
        private readonly ApiServer apiServer;

        public MainWindow()
        {
            Env.Load(@".env");
            saveTaskManager = new SaveTaskManager();
            // ViewModel
            saveTaskViewModel = new SaveTaskViewModel(saveTaskManager);
            Closing += saveTaskViewModel.OnWindowClosing;
            //DataContext
            DataContext = saveTaskViewModel;
            InitializeComponent();
            apiServer = new ApiServer(saveTaskViewModel, saveTaskManager);
            apiServer.Start();
        }

        private void AddRow_Click(object sender, RoutedEventArgs e)
        {
            //Windows
            saveTaskWindow = new(saveTaskViewModel);
            saveTaskWindow.ShowDialog();
        }
        private void StartSelected_Click(object sender, RoutedEventArgs e)
        { 
            var selectedRows = BackupTable.SelectedItems.Cast<SaveTask>().ToList();
            foreach (var row in selectedRows) 
            {
                saveTaskViewModel.ExecuteSaveTask(row.name);
            }
        }

        private void ModifySelected_Click(object sender, RoutedEventArgs e)
        {
            var selectedRows = BackupTable.SelectedItems.Cast<SaveTask>().ToList();
            foreach (var row in selectedRows)
            {
                saveTaskViewModel.SwitchSaveTaskType(row.name);
            }
        }

        private void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {
            var selectedRows = BackupTable.SelectedItems.Cast<SaveTask>().ToList();
            foreach(var row in selectedRows)
            {
                saveTaskViewModel.RemoveSaveTask(row.name);
            }
        }

        // Launches the settings window.
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            LaunchSettingsWindow();
        }

        public void BackupTable_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                // Récupérer l'objet modifié
                SaveTask modifiedTask = e.Row.Item as SaveTask;
                FrameworkElement test = e.EditingElement;
                if (modifiedTask != null && e.EditingElement is TextBox textBox)
                {
                    string name = modifiedTask.name;
                    string newValue = textBox.Text; // Récupérer la nouvelle valeur entrée par l'utilisateur
                    string columnName = e.Column.Header.ToString(); // Identifier la colonne modifiée
                    switch (columnName)
                    {
                        case "Nom":
                            if (!saveTaskViewModel.ModifySaveTaskName(name, newValue))
                            {
                                textBox.Text = modifiedTask.BindName;
                                return; //error here from ModifySaveTaskName
                            }
                            break;
                        case "Source":
                            if (!saveTaskViewModel.ModifySaveTaskSourcePath(name, newValue))
                            {
                                textBox.Text = modifiedTask.BindSource;
                                return; //error here from ModifySaveTaskName
                            }
                            break;
                        case "Destination":
                            if (!saveTaskViewModel.ModifySaveTaskTargetPath(name, newValue))
                            {
                                textBox.Text = modifiedTask.BindDestination;
                                return; //error here from ModifySaveTaskName
                            }
                            break;
                        case "Type":
                            if (!saveTaskViewModel.ModifySaveTaskType(name, ESaveTaskTypes.Complete))
                            {
                                textBox.Text = modifiedTask.BindDestination;
                                return; //error here from ModifySaveTaskName
                            }
                            break;
                    }
                }
            }
        }
        // Launches the settings window.
        public void LaunchSettingsWindow()
        {
            SettingsWindow settingsWindow = new SettingsWindow(saveTaskManager);
            try
            {
                settingsWindow.ShowDialog();
            }
            catch
            {
                settingsWindow.Close();
            }
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            await apiServer.StopAsync();
        }
    }
}