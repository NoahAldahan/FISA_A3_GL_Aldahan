using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasySaveWPFApp.Model;

namespace EasySaveWPFApp.ViewModel
{
    public class SaveTaskProgressViewModel : INotifyPropertyChanged
    {
        public SaveTaskManager saveTaskManager;
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<SaveTask> SaveTasksCurrentlySaving;
        public ObservableCollection<SaveTask> BindSaveTasksCurrentlySaving
        { get { return SaveTasksCurrentlySaving; }  set { SaveTasksCurrentlySaving = value; OnPropertyChanged(nameof(BindSaveTasksCurrentlySaving)); } }
        internal SaveTaskProgressViewModel(List<SaveTask> SaveTasksStarted, SaveTaskManager saveTaskManager)
        {
            SaveTasksCurrentlySaving = new ObservableCollection<SaveTask>(SaveTasksStarted);
            this.saveTaskManager = saveTaskManager;
        }


        public void PlaySaveTasks(List<SaveTask> SaveTasks)
        {
            foreach (var task in SaveTasks)
            {
                task.Play();
            }
        }
        public void PauseSaveTasks(List<SaveTask> SaveTasks)
        {
            foreach (var task in SaveTasks)
            {
                task.Pause();
            }
        }
        public void StopSaveTasks(List<SaveTask> SaveTasks)
        {
            foreach (var task in SaveTasks)
            {
                task.Stop();
            }
        }
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


    }
}
