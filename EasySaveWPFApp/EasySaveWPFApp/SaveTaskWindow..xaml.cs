using System;
using System.Windows;
using EasySaveWPFApp.ViewModel;
using EasySaveWPFApp.Model;
using System.Security.AccessControl;

namespace EasySaveWPFApp
{
    public partial class SaveTaskWindow : Window
    {
        SaveTaskViewModel saveTaskViewModel;
        public SaveTaskWindow(SaveTaskViewModel saveTaskViewModel)
        {
            InitializeComponent();
            this.saveTaskViewModel = saveTaskViewModel;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            ESaveTaskTypes eSaveTaskTypes;
            if (DifferentialRadioButton.IsChecked == true)
            {
                eSaveTaskTypes = ESaveTaskTypes.Differential;
            }
            else if (CompleteRadioButton.IsChecked == true)
            {
                eSaveTaskTypes = ESaveTaskTypes.Complete;
            }
            else
            {
                return;
            }
            this.saveTaskViewModel.CreateSaveTask(TaskName.Text, TaskSource.Text, TaskDestination.Text, eSaveTaskTypes);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();   
        }

        private void TaskSource_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void TaskName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }

        private void DifferentialRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
