using EasySaveConsole.Model;
using EasySaveConsole.View;
using System.Collections.Generic;
using EasySaveConsole.Utilities;
using System;
using System.Text.RegularExpressions;

namespace EasySaveConsole.Controller
{
    // Enum defining the possible CLI save task actions
    enum ECliSaveTaskAction
    {
        InitMenu = 0,    // Action to initialize the save task menu
        ShowTasks = 1,   // Action to display all save tasks
        StartTasks = 2,  // Action to start save tasks
        CreateTask = 3,  // Action to create a new save task
        ModifyTask = 4,  // Action to modify an existing save task
        DeleteTask = 5,  // Action to delete a save task
        Help = 6,        // Action to display help information
        Quit = 7         // Action to quit the save task menu
    }

    // Controller class for managing save tasks in the CLI
    internal class SaveTaskController : BaseController
    {
        // Manager for handling save tasks
        internal SaveTaskManager saveTaskManager;

        // Constructor for the SaveTaskController class
        internal SaveTaskController(MessageManager messagesManager, SaveTaskView view, SaveTaskManager saveTaskManager)
            : base(messagesManager, view)
        {
            this.saveTaskManager = saveTaskManager;
            stopCondition = (int)ECliSaveTaskAction.Quit; // Set the stop condition to the Quit action
            initCondition = (int)ECliSaveTaskAction.InitMenu; // Set the initial condition to the InitMenu action
            InitDictAction(); // Initialize the dictionary of actions
        }

        // Override the base method to initialize the dictionary of actions
        protected override void InitDictAction()
        {
            dictActions.Add((int)ECliSaveTaskAction.InitMenu, () => { ShowMessage(EMessage.MenuSaveTaskMessage); }); // Add action to show the save task menu
            dictActions.Add((int)ECliSaveTaskAction.Quit, () => { ShowMessage(EMessage.StopMessage); }); // Add action to quit the save task menu
            dictActions.Add((int)ECliSaveTaskAction.CreateTask, () => CreateSaveTask()); // Add action to create a new save task
            dictActions.Add((int)ECliSaveTaskAction.StartTasks, () => StartSaveTasks()); // Add action to start save tasks
            dictActions.Add((int)ECliSaveTaskAction.ShowTasks, () => WrapperShowAllSaveTask()); // Add action to display all save tasks
        }

        // Method to create a new save task
        internal void CreateSaveTask()
        {
            string saveTaskName = ShowQuestion(EMessage.AskSaveTaskNameMessage); // Ask the user for the save task name
            string saveTaskSource = ShowQuestion(EMessage.AskSaveTaskSourceFolderMessage); // Ask the user for the source folder
            if (!Utilities.Utilities.IsValidPath(saveTaskSource))
            {
                ShowMessagePause(EMessage.ErrorSaveTaskPathMessage); // Show an error message if the source path is invalid
                return;
            }
            string saveTaskTarget = ShowQuestion(EMessage.AskSaveTaskTargetFolderMessage); // Ask the user for the target folder
            if (!Utilities.Utilities.IsValidPath(saveTaskTarget))
            {
                ShowMessagePause(EMessage.ErrorSaveTaskPathMessage); // Show an error message if the target path is invalid
                return;
            }
            int saveTaskType = int.Parse(ShowQuestion(EMessage.AskSaveTaskType)); // Ask the user for the save task type
            if (!Enum.IsDefined(typeof(ESaveTaskTypes), saveTaskType))
            {
                ShowMessagePause(EMessage.ErrorSaveTaskTypeMessage); // Show an error message if the save task type is invalid
                return;
            }
            saveTaskManager.AddSaveTask((ESaveTaskTypes)saveTaskType, saveTaskSource, saveTaskTarget, saveTaskName); // Add the new save task
            ShowMessagePause(EMessage.SaveTaskAddSuccessMessage); // Show a success message
        }

        // Method to start save tasks
        internal void StartSaveTasks()
        {
            ShowAllSaveTask(); // Display all save tasks
            string saveTaskSave = ShowQuestion(EMessage.AskSaveTaskNameMessage); // Ask the user to select save tasks to start
            string patternRange = @"^\d+-\d+$"; // Regex pattern for a range of tasks (e.g., 1-3)
            string patternList = @"^\d+(;\d+)*$"; // Regex pattern for a list of tasks (e.g., 1;2;3)
            string patternSingle = @"^\d+$"; // Regex pattern for a single task (e.g., 1)
            if (Regex.IsMatch(saveTaskSave, patternRange))
                HandleSaveTaskRange(saveTaskSave); // Handle a range of save tasks
            else if (Regex.IsMatch(saveTaskSave, patternList))
                HandleSaveTaskList(saveTaskSave); // Handle a list of save tasks
            else if (Regex.IsMatch(saveTaskSave, patternSingle))
                HandleSaveTask(saveTaskSave); // Handle a single save task
            else
                ShowQuestion(EMessage.PressKeyToContinue); // Wait for user confirmation if the input is invalid
        }

        // Method to handle a range of save tasks
        internal void HandleSaveTaskRange(string saveTaskSave)
        {
            string[] rangeParts = saveTaskSave.Split('-'); // Split the range string into start and end values
            int start = int.Parse(rangeParts[0]); // Parse the start value
            int end = int.Parse(rangeParts[1]); // Parse the end value
            saveTaskManager.ExecuteSaveTaskRange(start, end); // Execute the range of save tasks
        }

        // Method to handle a list of save tasks
        internal void HandleSaveTaskList(string saveTaskSave)
        {
            string[] valuesStr = saveTaskSave.Split(';'); // Split the list string into individual values
            List<int> indexs = new List<int>(); // List to store the parsed indices
            try
            {
                foreach (string val in valuesStr)
                {
                    indexs.Add(int.Parse(val)); // Parse each value and add it to the list
                }
            }
            catch
            {
                Console.WriteLine("Your input is not an integer"); // Show an error message if parsing fails
            }
            Console.WriteLine($"Detected list of save tasks: {string.Join(", ", valuesStr)}"); // Display the detected list of save tasks
            saveTaskManager.ExecuteSaveTaskList(indexs); // Execute the list of save tasks
            ShowQuestion(EMessage.PressKeyToContinue); // Wait for user confirmation
        }

        // Method to handle a single save task
        internal void HandleSaveTask(string saveTaskSave)
        {
            Console.WriteLine($"Detected single save task: {saveTaskSave}"); // Display the detected single save task
            saveTaskManager.ExecuteSaveTask(int.Parse(saveTaskSave)); // Execute the single save task
            ShowQuestion(EMessage.PressKeyToContinue); // Wait for user confirmation
        }

        // Method to display all save tasks
        internal void ShowAllSaveTask()
        {
            List<SaveTask> saveTasks = saveTaskManager.GetAllSaveTask(); // Get all save tasks
            ShowMessage(EMessage.ShowSaveTaskRegisterMessage); // Show a message indicating the list of save tasks
            int id = 0; // Initialize the task ID
            foreach (SaveTask saveTask in saveTasks)
            {
                ShowMessage($" {id} : {saveTask.name}"); // Display each save task with its ID and name
                id++; // Increment the task ID
            }
        }

        // Wrapper method to display all save tasks and wait for user confirmation
        internal void WrapperShowAllSaveTask()
        {
            ShowAllSaveTask(); // Display all save tasks
            ShowQuestion(EMessage.PressKeyToContinue); // Wait for user confirmation
        }
    }
}
