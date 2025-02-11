```mermaid
classDiagram

%% ==================== Model ====================
class SaveTaskFactory {
    ~ CreateSave(saveTaskTypes: ESaveTaskTypes, sourcePath: string, targetPath: string): SaveTask
}

class SaveTask {
    ~ DirectoryPair CurrentDirectoryPair
    ~ SaveTask(DirectoryPair CurrentDirectoryPair)
    ~ DirectoryPair GetDirectoryPair()
    ~ abstract void Save()
    ~ abstract List<string> GetInfo()
}

class SaveTaskDifferential {
    ~internal SaveTaskDifferential(DirectoryPair CurrentDirectoryPair)
    ~internal void Save()
    ~internal List<string> GetInfo()
    -private void SaveDifferentialRecursive(string SourcePath, string TargetPath)
}

class SaveTaskComplete {
    ~ SaveTaskComplete(DirectoryPair CurrentDirectoryPair)
    ~ override void Save()
    - void CopyFilesRecursivelyForTwoFolders(DirectoryInfo sourceDirectoryInfo, DirectoryInfo targetDirectoryInfo)
    ~ override List<string> GetInfo()
}

class DirectoryPair {
    ~ SourcePath: string
    ~ TargetPath: string
    ~ DirectoryPair(SourcePath: string, TargetPath: string)
}

class SaveTaskManager {
    - static MaxSaveTasks: int = 5
    ~ SaveTasks: List<SaveTask>
    ~ SaveTaskFactory: SaveTaskFactory
    ~ GetSaveTasksClone(): List<SaveTask>
    ~ SaveTaskManager()
    ~ AddSaveTask(SaveTaskType: ESaveTaskTypes, sourcePath: string, targetPath: string): void
    ~ RemoveSaveTask(index: int): void
    ~ ExecuteSaveTask(index: int): void
    ~ ExecuteAllSaveTasks(): void
}

class LanguageManager {
    ~ ELanguage defaultLanguage
    - JsonManager jsonManager
    ~ LanguageManager()
    ~ EMessage InitDefaultLanguage()
    ~ static bool IsValidLanguage(string language)
    ~ EMessage SetDefaultLanguage(string languageValue)
    ~ ELanguage GetLanguageInstance(string language)
}

class MessageManager {
    ~ LanguageManager languageManager
    ~ JsonManager jsonManager
    + MessageManager(LanguageManager languageManager, JsonManager jsonManager)
    + string GetMessageTranslate(EMessage message)
}

class Log {
    +message: String
    +writeLog(message: String)
}

class JsonManager {
    +readJson(file: String): Object
    +writeJson(file: String, data: Object)
}

class IObserver {
    
}

%% Relations dans Model
SaveTaskFactory --* SaveTask : "create"
SaveTask --* DirectoryPair : "create"
SaveTask <|-- SaveTaskDifferential
SaveTask <|-- SaveTaskComplete
MessageManager o-- JsonManager
SaveTaskDifferential --|> IObserver
SaveTaskComplete --|> IObserver
LanguageManager --> MessageManager
SaveTaskManager --> SaveTask 
SaveTaskFactory --> DirectoryPair
Log o-- JsonManager
SaveTask --> Log : "use"

%% ==================== Controller ====================
class CliController {
    ~ ECliAction action
    ~ SaveTaskController saveTaskController
    ~ CliController(MessageManager messageManager, CliView view, SaveTaskController saveTaskController, LanguageController languageController)
    ~ void HandleUserInput()
    ~ void StartCli()
}

class LogController {
    +logData: String
    +addLog(message: String)
    +getLogs(): List<String>
}

class SaveTaskController {
    ~ ECliSaveTaskAction action
    ~ SaveTaskManager saveTaskManager
    ~ SaveTaskController(MessageManager messageManager, SaveTaskView view, SaveTaskManager saveTaskManager)
    ~ void HandleUserInput()
    ~ void StartCli()
}

class BaseController {
    # MessageManager messageManager
    # BaseView view
    # BaseController(MessageManager messageManager, BaseView view)
    # void ShowMessage(Message msg)
    # string ShowQuestion(Message msg)
}

class LanguageController {
    ~ LanguageManager langManager
    ~ LanguageController(LanguageManager langManager)
    ~ Message SetDefaultLanguage(string strLanguage)
}

%% Relations dans Controller
SaveTaskController o-- SaveTaskManager : "manage"
SaveTaskManager *-- SaveTaskFactory : "manage"
Log <-- LogController : "manages"
CliController o-- LogController
CliController o-- SaveTaskController
CliController --> ECliAction
SaveTaskController --> ECliSaveTaskAction
SaveTaskController --|> BaseController
LogController --|> BaseController
CliController --|> BaseController
LanguageController --|> BaseController
BaseController o-- MessageManager
CliController o-- LanguageController
LanguageController o-- LanguageManager
LanguageManager o-- JsonManager

%% ==================== View ====================
class CliView {
    
}

class BaseView {
    + void ShowMessage(string message)
    + string GetUserInput(string input)
    + int GetOptionUserInput()
    + string GetUserInput()
    + string ShowQuestion(string question)
}

class SaveTaskView {
 + void ShowMessage(string message)
}
class LanguageView {
 + void ShowMessage(string message)
}
class LogView {
     + void ShowMessage(string message)
}

%% Relations dans View
CliController o-- CliView
SaveTaskController o-- SaveTaskView
LanguageController o-- LanguageView
CliView --|> BaseView
SaveTaskView --|> BaseView
LanguageView --|> BaseView
LogView --|> BaseView
BaseController o-- BaseView
LogController o-- LogView
