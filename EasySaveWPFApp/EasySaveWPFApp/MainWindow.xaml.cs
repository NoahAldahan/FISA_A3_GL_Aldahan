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
using EasySaveWPFApp.Controller;
using EasySaveWPFApp.Model;
using EasySaveWPFApp.Utilities;
using EasySaveWPFApp.View;

namespace EasySaveWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        SaveTaskController saveTaskController;
        MessageManager messageManager;
        public MainWindow()
        {
            InitializeComponent();
            Env.Load(@".env");
            LanguageManager languageManager = new LanguageManager();
            messageManager = new MessageManager(languageManager);
            SaveTaskView view = new SaveTaskView();
            SaveTaskManager saveTaskManager = new SaveTaskManager();
            saveTaskManager.SaveTasks.Add(new SaveTaskComplete(new DirectoryPair("C:", "C:"), "name"));
            saveTaskController = new SaveTaskController(messageManager, view, saveTaskManager);
            DataContext = saveTaskController.saveTaskManager;
        }

        private void AddRow_Click(object sender, RoutedEventArgs e)
        {

        }
        private void StartSelected_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ModifySelected_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteSelected_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsWindow settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog(); // Ouvre la fenêtre et bloque l'autre jusqu'à fermeture
        }

        private void BackupTable_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}