using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;

namespace EasySaveWPFApp
{
    public partial class UnsavedFilesWindow : Window
    {
        public ObservableCollection<KeyValuePair<string, List<string>>> UnsavedFiles { get; set; }

        public UnsavedFilesWindow(Dictionary<string, List<string>> unsavedPathsDictionary)
        {
            InitializeComponent();
            UnsavedFiles = new ObservableCollection<KeyValuePair<string, List<string>>>(unsavedPathsDictionary);
            DataContext = this;
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}

