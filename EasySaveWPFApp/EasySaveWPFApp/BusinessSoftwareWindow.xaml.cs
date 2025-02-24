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
using EasySaveWPFApp.Model;
using EasySaveWPFApp.Utilities;
using System.Collections.ObjectModel;

namespace EasySaveWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class BusinessSoftwareWindow : Window
    {
        public BusinessSoftwareWindow()
        {
        InitializeComponent(); // Appelle la méthode générée par le XAML
        }

        private void ClosePopup(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}