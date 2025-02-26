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
using EasySaveWPFApp.Utilities;
using System.Collections.ObjectModel;
using Log;
using System.Diagnostics;

namespace EasySaveWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class SaveTaskProgressWindow : Window
    {
        SaveTaskProgressViewModel saveTaskProgressViewModel;
        Dictionary<string, List<string>> UnsavedPathsDictionary;
        List<Task> SaveTaskTasksReferences;

        public SaveTaskProgressWindow(List<SaveTask> SaveTasksStarted, SaveTaskViewModel saveTaskViewModel, SaveTaskManager saveTaskManager)
        {
            // ViewModel
            saveTaskProgressViewModel = new SaveTaskProgressViewModel(SaveTasksStarted, saveTaskManager);
            //DataContext
            DataContext = saveTaskProgressViewModel;
            InitializeComponent();
            SaveTaskTasksReferences = new List<Task>();
            UnsavedPathsDictionary = new Dictionary<string, List<string>>();
            ExecuteAllSaveTasksAsync(SaveTasksStarted, saveTaskViewModel);
        }

        private async void ExecuteAllSaveTasksAsync(List<SaveTask> SaveTasksStarted, SaveTaskViewModel saveTaskViewModel)
        {
            foreach (var row in SaveTasksStarted)
            {
                SaveTaskTasksReferences.Add(ExecuteSaveTaskAsync(row.name, SaveTasksStarted, saveTaskViewModel));
            }
            await Task.WhenAll(SaveTaskTasksReferences); // Attend que toutes les tâches soient terminées

            OnAllTasksCompleted();
        }

        private void OnAllTasksCompleted()
        {
            CheckAndShowSaveTaskStoppedWindow();
            CheckAndShowUnsavedFilesWindow();
        }

        private async Task ExecuteSaveTaskAsync(string name, List<SaveTask> SaveTasksStarted, SaveTaskViewModel saveTaskViewModel)
        {
            Dictionary<string, List<string>> newUnsavedPathsLists = await saveTaskViewModel.ExecuteSaveTaskAsync(name);
            Trace.WriteLine("ExecuteSaveTaskAsync: end await new unsavedpathlists");
            foreach (var unsavedPathLists in newUnsavedPathsLists)
            {
                if(! UnsavedPathsDictionary.ContainsKey(unsavedPathLists.Key))
                {
                    UnsavedPathsDictionary.Add(unsavedPathLists.Key, unsavedPathLists.Value);
                }
            }
        }

        private void PlaySelected_Click(object sender, RoutedEventArgs e)
        {
            var selectedRows = ProgressTable.SelectedItems.Cast<SaveTask>().ToList();
            saveTaskProgressViewModel.PlaySaveTasks(selectedRows);
        }
        private void PauseSelected_Click(object sender, RoutedEventArgs e)
        {
            var selectedRows = ProgressTable.SelectedItems.Cast<SaveTask>().ToList();
            saveTaskProgressViewModel.PauseSaveTasks(selectedRows);
        }
        private void StopSelected_Click(object sender, RoutedEventArgs e)
        {
            var selectedRows = ProgressTable.SelectedItems.Cast<SaveTask>().ToList();
            saveTaskProgressViewModel.StopSaveTasks(selectedRows);
        }

        private void CheckAndShowUnsavedFilesWindow()
        {
            if (UnsavedPathsDictionary.Count > 0)
            {
                UnsavedFilesWindow unsavedFilesWindow = new UnsavedFilesWindow(UnsavedPathsDictionary);
                unsavedFilesWindow.ShowDialog();
            }
        }

        private void CheckAndShowSaveTaskStoppedWindow()
        {
            List<SaveTask> stoppedSaveTask = new List<SaveTask>();
            foreach(var item in saveTaskProgressViewModel.BindSaveTasksCurrentlySaving) 
            {
                if (item.nFilesUnsavedCancelled > 0)
                {
                    stoppedSaveTask.Add(item);
                }
            }
            if(stoppedSaveTask.Count > 0)
            {
                Trace.WriteLine("entered stoppedSaveTask.Count > 0 with stoppedSaveTask.count = " + stoppedSaveTask.Count.ToString());
                Trace.WriteLine("entered stoppedSaveTask.Count > 0 with  " + stoppedSaveTask[0].name + "   " + stoppedSaveTask[0].CurrentDirectoryPair.SourcePath + "  " + stoppedSaveTask[0].CurrentDirectoryPair.TargetPath);
                SaveTaskStoppedWindow saveTaskStoppedWindow = new SaveTaskStoppedWindow(stoppedSaveTask);
                saveTaskStoppedWindow.ShowDialog();
                stoppedSaveTask.Clear();
            }
        }
    }
}