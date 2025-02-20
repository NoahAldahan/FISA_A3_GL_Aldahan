using EasySaveWPFApp.ViewModel;
using EasySaveWPFApp.Model;
using System.Configuration;
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
using static System.Net.Mime.MediaTypeNames;

namespace EasySaveWPFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        SettingsViewModel settingsViewModel ;

        // Constructor: Initializes the settings window
        public SettingsWindow(SaveTaskManager saveTaskManager)
        {
            InitializeComponent();

            settingsViewModel = new SettingsViewModel(saveTaskManager);

            FrenchRadioButton.IsChecked = settingsViewModel.ShouldFrenchRadioButtonBeChecked();
            EnglishRadioButton.IsChecked = settingsViewModel.ShouldEnglishRadioButtonBeChecked();

            JSONRadioButton.IsChecked = settingsViewModel.ShouldJSONRadioButtonBeChecked();
            XMLRadioButton.IsChecked = settingsViewModel.ShouldXMLRadioButtonBeChecked();

            RefreshEncryptingExtensionsDisplay();
        }

        // Language radio buttons
        private void FrenchLanguageRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            settingsViewModel.AddLanguageAction(ESettingsActions.SwitchLanguageToFrench);
        }

        private void EnglishLanguageRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            settingsViewModel.AddLanguageAction(ESettingsActions.SwitchLanguageToEnglish);
        }

        private void XMLRadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        // Save and cancel buttons
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            settingsViewModel.ExecuteAllUnsavedActions();
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // If cancel button is clicked, don't do any changes to settings
            this.Close(); // Ferme la fenêtre des paramètres
        }

        // Validate button for Encrypting extensions
        private void ValidateExtensionButton(object sender, RoutedEventArgs e)
        {
            string EncryptingExtensionText = EncryptingExtensionTextBox.Text.Trim();
            // TODO : Popup for error message
            bool wasAdded = settingsViewModel.AddEncryptingExtension(EncryptingExtensionText);
            EncryptingExtensionTextBox.Text = "";
            RefreshEncryptingExtensionsDisplay();
        }

        // Display the list of encrypting extensions
        // Dynamically generates extensions display
        private void RefreshEncryptingExtensionsDisplay()
        {
            List<string> encryptingExtensions = settingsViewModel.GetNewEncryptingExtensions();
            EncryptingExtensionsStackPanel.Children.Clear();
            foreach (string extension in encryptingExtensions)
            {
                /*
                 * <Border BorderBrush="Black" BorderThickness="2" Height="35" Margin="5,0,0,0">
                      <StackPanel Orientation="Horizontal" HorizontalAlignment="Left" VerticalAlignment="Top">
                         <Button Content="X" Margin="5,5,5,5" Height ="20" Width="20" Click="Cancel_Click"/>
                         <TextBlock Text=".doc" Margin="5,5,5,5" HorizontalAlignment="Center" VerticalAlignment="Center"/>
                      </StackPanel>
                   </Border>
                 * */
                Border border = new Border();
                border.BorderBrush = Brushes.Black;
                border.BorderThickness = new Thickness(2);
                border.Height = 35;
                border.Margin = new Thickness(5, 0, 0, 0);
                EncryptingExtensionsStackPanel.Children.Add(border);

                StackPanel stackPanel = new StackPanel();
                stackPanel.Orientation = Orientation.Horizontal;
                stackPanel.HorizontalAlignment = HorizontalAlignment.Left;
                stackPanel.VerticalAlignment = VerticalAlignment.Top;
                border.Child = stackPanel;

                Button deleteButton = new Button();
                deleteButton.Content = "X";
                deleteButton.Margin = new Thickness(5);
                deleteButton.Height = 20;
                deleteButton.Width = 20;
                deleteButton.Tag = extension;
                deleteButton.Click += DeleteExtensionButton_Click;
                stackPanel.Children.Add(deleteButton);

                TextBlock textBlock = new TextBlock();
                textBlock.Text = extension;
                textBlock.VerticalAlignment = VerticalAlignment.Center;
                textBlock.HorizontalAlignment = HorizontalAlignment.Center;
                textBlock.Margin = new Thickness(5);
                textBlock.Tag = extension;
                stackPanel.Children.Add(textBlock);
            }
        }

        // Delete an extension from the list of encrypting extensions
        private void DeleteExtensionButton_Click(object sender, RoutedEventArgs e)
        {
            Button senderButton = (Button)sender;
            string ExtensionTag = (string)senderButton.Tag;
            settingsViewModel.RemoveEncryptingExtension(ExtensionTag);
            RefreshEncryptingExtensionsDisplay();
        }
    }
}