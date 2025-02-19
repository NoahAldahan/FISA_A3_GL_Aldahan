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

namespace EasySaveWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SaveTaskManager saveTaskManager;

        public MainWindow()
        {
            InitializeComponent();
            saveTaskManager = new SaveTaskManager();
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

        // Launches the settings window.
        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            LaunchSettingsWindow();
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
    }
}