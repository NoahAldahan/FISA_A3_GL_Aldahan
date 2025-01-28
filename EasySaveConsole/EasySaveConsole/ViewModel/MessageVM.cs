using EasySaveConsole.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.ViewModel
{
    internal class MessageVM : INotifyPropertyChanged
    {
        private readonly MessagesManager messagesManager;
        private string currentMessage;
        private Messages selectedMessage;
        public MessageVM() 
        {
            messagesManager = new MessagesManager();
        }

        public string CurrentMessage
        {
            get => currentMessage;
            private set
            {
                if(currentMessage != value)
                {
                    currentMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        private void LoadMessage(Messages message)
        {
            try
            {
                CurrentMessage = messagesManager.GetMessage(message);
            }
            catch (Exception ex)
            {
                CurrentMessage = $"Error: {ex.Message}";
            }
        }

        public Messages SelectedMessage
        {
            get => SelectedMessage;
            set
            {
                if (selectedMessage != value)
                {
                    selectedMessage = value;
                    LoadMessage(value);
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
