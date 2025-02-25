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

        public SaveTaskProgressWindow(List<SaveTask> SaveTasksStarted, SaveTaskViewModel saveTaskViewModel, SaveTaskManager saveTaskManager)
        {
            // ViewModel
            saveTaskProgressViewModel = new SaveTaskProgressViewModel(SaveTasksStarted, saveTaskManager);
            //DataContext
            DataContext = saveTaskProgressViewModel;
            InitializeComponent();
            ExecuteSaveTasks(SaveTasksStarted, saveTaskViewModel);
            UnsavedPathsDictionary = new Dictionary<string, List<string>>();
        }

        private void ExecuteSaveTasks(List<SaveTask> SaveTasksStarted, SaveTaskViewModel saveTaskViewModel)
        {
            foreach (var row in SaveTasksStarted)
            {
                ExecuteSaveTaskAsync(row.name, SaveTasksStarted, saveTaskViewModel);
            }
        }
        private async void ExecuteSaveTaskAsync(string name, List<SaveTask> SaveTasksStarted, SaveTaskViewModel saveTaskViewModel)
        {
            Dictionary<string, List<string>> newUnsavedPathsLists = await saveTaskViewModel.ExecuteSaveTaskAsync(name);
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

    }
}