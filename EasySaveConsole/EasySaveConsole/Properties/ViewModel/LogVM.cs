using System;
using System.Collections.Generic;

public class LogVM
{
    // Attribut pour stocker les logs
    private List<string> logData;

    // Constructeur de la classe LogVM
    public LogVM()
    {
        logData = new List<string>();
    }

    // M�thode pour ajouter un log
    public void AddLog(string message)
    {
        logData.Add(message);  // Ajoute le message � la liste des logs
    }

    // M�thode pour obtenir la liste des logs
    public List<string> GetLogs()
    {
        return new List<string>(logData);  // Retourne une copie de la liste des logs
    }
}
