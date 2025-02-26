using System;
using System.Threading;

namespace CryptoSoft;

public static class CryptoSoft
{
    private static readonly Mutex mutex = new(false, "CryptoSoft_Instance");

    public static void Main(string[] args)
    {
        // Vérifie si une autre instance de l'application est déjà en cours d'exécution
        if (!mutex.WaitOne(TimeSpan.Zero, true))
        {
            Console.WriteLine("CryptoSoft is already running. Only one instance is allowed.");
            return;
        }

        try
        {
            RunApplication();
        }
        finally
        {
            // Libère le mutex lorsque l'application se ferme
            mutex.ReleaseMutex();
        }
    }

    private static void RunApplication()
    {
        bool continueLoop = true;

        try
        {
            while (continueLoop)
            {
                Console.WriteLine("Enter the path of the file to encrypt/decrypt:");
                string? path = Console.ReadLine()?.Trim();

                Console.WriteLine("Enter the key to encrypt/decrypt the file:");
                string? key = Console.ReadLine()?.Trim();

                if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(key))
                {
                    Console.WriteLine("Invalid input. Please enter a valid file path and key.");
                    continue;
                }

                int elapsedTime = EncryptFile(path, key);
                if (elapsedTime == -99)
                {
                    Console.WriteLine("An error occurred while encrypting or decrypting the file.");
                }
                else
                {
                    Console.WriteLine("File encrypted/decrypted in " + elapsedTime + " ms.");
                }

                continueLoop = AskToContinue();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"An unexpected error occurred: {e.Message}");
            Environment.Exit(-99);
        }
    }

    private static int EncryptFile(string path, string key)
    {
        try
        {
            var fileManager = new FileManager(path, key);
            return fileManager.TransformFile();
        }
        catch
        {
            return -99;
        }
    }

    private static bool AskToContinue()
    {
        while (true)
        {
            Console.WriteLine("Do you want to encrypt/decrypt another file? (y/n)");
            string? answer = Console.ReadLine()?.Trim().ToLower();

            if (answer == "n" || answer == "no")
                return false;
            if (answer == "y" || answer == "yes")
                return true;

            Console.WriteLine("Invalid input. Please enter 'y' or 'n'.");
        }
    }
}
