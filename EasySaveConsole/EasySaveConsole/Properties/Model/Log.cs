using System;
using System.IO;
using Newtonsoft.Json;

public class Log
{
    // Chemin de base pour les fichiers journaliers
    private readonly string logDirectoryPath = "Logs";

    public Log()
    {
        // Cr�er le r�pertoire des logs s'il n'existe pas
        if (!Directory.Exists(logDirectoryPath))
        {
            Directory.CreateDirectory(logDirectoryPath);
        }
    }

    // M�thode pour �crire une entr�e dans le fichier journal
    public void WriteLog(string backupName, string sourcePath, string destinationPath, long fileSize, long transferTimeMs)
    {
        // Nom du fichier log bas� sur la date du jour
        string logFileName = $"{DateTime.Now:yyyy-MM-dd}.json";
        string logFilePath = Path.Combine(logDirectoryPath, logFileName);

        // Construire l'objet du log
        var logEntry = new
        {
            Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            BackupName = backupName,
            SourcePath = sourcePath,
            DestinationPath = destinationPath,
            FileSize = fileSize,
            TransferTimeMs = transferTimeMs
        };

        try
        {
            // Convertir l'objet en JSON
            string jsonEntry = JsonConvert.SerializeObject(logEntry, Formatting.Indented);

            // �criture dans le fichier
            using (StreamWriter writer = new StreamWriter(logFilePath, true))
            {
                writer.WriteLine(jsonEntry);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de l'�criture dans le fichier journal : {ex.Message}");
        }
    }
}
