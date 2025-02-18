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
using EasySaveWPFApp.Model;
using EasySaveWPFApp.Utilities;

namespace EasySaveWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        SaveTaskViewModel saveTaskViewModel;
        SaveTaskWindow saveTaskWindow;
        public MainWindow()
        {
            InitializeComponent();
            Env.Load(@".env");
            SaveTaskManager saveTaskManager = new SaveTaskManager();
            //Controller
            saveTaskViewModel = new SaveTaskViewModel(saveTaskManager);
            //DataContext
            DataContext = saveTaskViewModel;
        }

        private void AddRow_Click(object sender, RoutedEventArgs e)
        {
            //Windows
            saveTaskWindow = new(saveTaskViewModel);
            saveTaskWindow.ShowDialog();
        }
        private void StartSelected_Click(object sender, RoutedEventArgs e)
        { }

        private void ModifySelected_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteSelected_Click(object sender, RoutedEventArgs e)
        { }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog(); // Ouvre la fenêtre et bloque l'autre jusqu'à fermeture
        }

        private void BackupTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { }

        public void BackupTable_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                // Récupérer l'objet modifié
                SaveTask modifiedTask = e.Row.Item as SaveTask;
            }
        }
    }
}