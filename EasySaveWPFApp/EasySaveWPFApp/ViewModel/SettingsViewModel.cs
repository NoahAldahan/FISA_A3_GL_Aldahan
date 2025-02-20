using EasySaveWPFApp.Model;
using EasySaveWPFApp.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EasySaveWPFApp.ViewModel
{
    enum ESettingsActions
    {
        SwitchLanguageToFrench,
        SwitchLanguageToEnglish,
        SwitchLogTypeToJSON,
        SwitchSaveTypeToXML,
        EditedExtensions
    }
    public class SettingsViewModel
    {
        // The list of unsaved actions (the user needs to click save to save them)
        List<ESettingsActions> UnsavedActions = new List<ESettingsActions>();
        // The new unsaved list of encrypting extensions (the user needs to click save to save them)
        List<string> NewEncryptingExtensions = new List<string>();
        SaveTaskManager saveTaskManager;

        // Constructor: Initializes the settings controller
        internal SettingsViewModel(SaveTaskManager saveTaskManager)
        {
            this.saveTaskManager = saveTaskManager;
            NewEncryptingExtensions = this.saveTaskManager.GetEncryptingExtensions();
        }

        // Execute all actions that are in the list of unsaved actions
        internal void ExecuteAllUnsavedActions()
        {
            // We put all language actions to the bottom of the list because changing language requires
            // to reload all windows, which would interrupt other actions
            PutAllLanguageActionsToBottomOfList();
            foreach (ESettingsActions action in UnsavedActions)
            {
                switch (action)
                {
                    case ESettingsActions.SwitchLogTypeToJSON:
                        LogUtilities.SetLogFormat(ELogFormat.JSON);
                        break;
                    case ESettingsActions.SwitchSaveTypeToXML:
                        LogUtilities.SetLogFormat(ELogFormat.XML);
                        break;
                    case ESettingsActions.EditedExtensions:
                        saveTaskManager.SetEncryptingExtensions(NewEncryptingExtensions);
                        break;
                    case ESettingsActions.SwitchLanguageToFrench:
                        // TODO :popup : this will restart the application, Continue ? Yes/No
                        SetLanguage("fr-FR");
                        break;
                    case ESettingsActions.SwitchLanguageToEnglish:
                        // TODO :popup : this will restart the application, Continue ? Yes/No
                        SetLanguage("en-US");
                        break;
                }
            }
            UnsavedActions.Clear();
        }

        // Put all language actions to the bottom of the list
        private void PutAllLanguageActionsToBottomOfList()
        {
            TryPutActionToBottomOfList(ESettingsActions.SwitchLanguageToFrench);
            TryPutActionToBottomOfList(ESettingsActions.SwitchLanguageToEnglish);
        }

        // Try to put the action at the bottom of the list
        private void TryPutActionToBottomOfList(ESettingsActions eSettingsActions)
        {
            if (UnsavedActions.Contains(eSettingsActions))
            {
                UnsavedActions.Remove(eSettingsActions);
                UnsavedActions.Add(eSettingsActions);
            }
        }

        // Add a language action to the list of unsaved actions
        // Removes actions that are the opposite of the added action
        internal void AddLanguageAction(ESettingsActions eSettingsActions)
        {
            if (!UnsavedActions.Contains(eSettingsActions) && IsLanguageDifferentToCurrent(eSettingsActions))
            {
                UnsavedActions.Add(eSettingsActions);
            }
            foreach (ESettingsActions action in new List<ESettingsActions>(UnsavedActions))
            {
                if (action == ESettingsActions.SwitchLanguageToFrench && eSettingsActions == ESettingsActions.SwitchLanguageToEnglish)
                {
                    UnsavedActions.Remove(action);
                }
                if (action == ESettingsActions.SwitchLanguageToEnglish && eSettingsActions == ESettingsActions.SwitchLanguageToFrench)
                {
                    UnsavedActions.Remove(action);
                }
            }
        }

        private bool IsLanguageDifferentToCurrent(ESettingsActions eSettingsActions)
        {
            string CurrentCulture = Thread.CurrentThread.CurrentCulture.Name;
            switch (eSettingsActions)
            {
                case ESettingsActions.SwitchLanguageToFrench:
                    return CurrentCulture != "fr-FR";
                case ESettingsActions.SwitchLanguageToEnglish:
                    return CurrentCulture != "en-US";
                default:
                    return false;
            }
        }

        // Set the language of the application
        public void SetLanguage(string cultureCode)
        {
            LanguageResourceViewModel.SetLanguage(cultureCode);
            ReloadMainWindow();
        }


        // Reloads the main windows and closes every other ones to refresh the text's language
        public static void ReloadMainWindow()
        {
            Application.Current.MainWindow.Close();
            MainWindow newWindow = new MainWindow();
            Application.Current.MainWindow = newWindow;
            newWindow.Show();
            foreach (Window window in Application.Current.Windows)
            {
                if (!(window is MainWindow))
                {
                    window.Close();
                }
            }
        }
        // Returns true if the English radio button should be checked
        internal bool ShouldEnglishRadioButtonBeChecked()
        {
            return Thread.CurrentThread.CurrentCulture.Name == "en-US";
        }

        // Returns true if the French radio button should be checked
        internal bool ShouldFrenchRadioButtonBeChecked()
        {
            return Thread.CurrentThread.CurrentCulture.Name == "fr-FR";
        }

        // Returns true if the JSON radio button should be checked
        internal bool ShouldJSONRadioButtonBeChecked()
        {
            return LogUtilities.GetLogFormat() == ELogFormat.JSON;
        }

        // Returns true if the XML radio button should be checked
        internal bool ShouldXMLRadioButtonBeChecked()
        {
            return LogUtilities.GetLogFormat() == ELogFormat.XML;
        }

        // Try to add the extension to the list of unsaved extensions
        // Returns true if the extension was added, false if it was already in the list
        public bool AddEncryptingExtension(string EncryptingExtension)
        {
            if (NewEncryptingExtensions.Contains(EncryptingExtension) || !EncryptingExtension.StartsWith(".") || EncryptingExtension.Length < 2)
            {
                return false;
            }
            NewEncryptingExtensions.Add(EncryptingExtension);
            TryAddEditedExtensionsAction();
            return true;
        }

        // Try to remove the extension from the list of unsaved extensions
        // Returns true if the extension was removed, false if it wasn't in the list
        public bool RemoveEncryptingExtension(string EncryptingExtension)
        {
            if (!NewEncryptingExtensions.Contains(EncryptingExtension))
            {
                return false;
            }
            NewEncryptingExtensions.Remove(EncryptingExtension);
            TryAddEditedExtensionsAction();
            return true;
        }

        // Returns the new unsaved list of encrypting extensions
        public List<string> GetNewEncryptingExtensions()
        {
            return new List<string> (NewEncryptingExtensions);
        }

        // Try to add the action EditedExtensions to the list of unsaved actions
        private void TryAddEditedExtensionsAction()
        {
            // If the two lists are equal (same elements in whatever order)
            List<string> savedEncryptingExtensions = saveTaskManager.GetEncryptingExtensions();
            if (!TwoListEqualsAnyOrder(NewEncryptingExtensions, saveTaskManager.GetEncryptingExtensions()))
            {
                UnsavedActions.Add(ESettingsActions.EditedExtensions);
            }
            else if (UnsavedActions.Contains(ESettingsActions.EditedExtensions))
            {
                UnsavedActions.Remove(ESettingsActions.EditedExtensions);
            }
        }

        // Returns true if the two lists are equal (same elements in whatever order)
        private bool TwoListEqualsAnyOrder(List<string> list1, List<string> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }

            foreach (string item in list1)
            {
                if (!list2.Contains(item))
                {
                    return false;
                }
            }
            return true;
        }

        internal void TrySwitchLogFormatToJSON()
        {
            if(LogUtilities.GetLogFormat() != ELogFormat.JSON && !UnsavedActions.Contains(ESettingsActions.SwitchLogTypeToJSON))
            {
                UnsavedActions.Add(ESettingsActions.SwitchLogTypeToJSON);
            }
        }

        internal void TrySwitchLogFormatToXML()
        {
            if (LogUtilities.GetLogFormat() != ELogFormat.XML && !UnsavedActions.Contains(ESettingsActions.SwitchSaveTypeToXML))
            {
                UnsavedActions.Add(ESettingsActions.SwitchSaveTypeToXML);
            }
        }
    }
}
