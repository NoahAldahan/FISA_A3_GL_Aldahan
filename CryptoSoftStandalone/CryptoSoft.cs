namespace CryptoSoft;

public static class CryptoSoft
{
    public static void Main(string[] args)
    {
        bool continueLoop = true;

        try
        {
            while (continueLoop)
            {
                Console.WriteLine("Enter the path of the file to encrypt/decrypt:");
                string? path = Console.ReadLine();
                Console.WriteLine("Enter the key to encrypt/decrypt the file:");
                string? key = Console.ReadLine();
                int ElapsedTime = EncryptFile(path, key);
                if (ElapsedTime == -99)
                {
                    Console.WriteLine("An error occurred while encrypting or decrypting the file.");
                }
                Console.WriteLine("File encrypted/decrypted in " + ElapsedTime + " ms.");
                string? answer = "";
                while (answer == "")
                {
                    Console.Clear();
                    Console.WriteLine("Do you want to encrypt/decrypt another file? (y/n)");
                    answer = Console.ReadLine();
                    if (answer == "n" || answer == "no" || answer == "No" || answer == "N" || answer == "NO")
                        continueLoop = false;
                    else if (answer == "y" || answer == "yes" || answer == "Yes" || answer == "Y" || answer == "YES")
                        continueLoop = true;
                    else
                        answer = "";
                }

            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Environment.Exit(-99);
        }
    }

    private static int EncryptFile(string ?path, string ?key)
    {
        if (path == null || key == null)
        {
            return -99;
        }
        try
        {
            var fileManager = new FileManager(path, key);
            int ElapsedTime = fileManager.TransformFile();
            return ElapsedTime;
        }
        catch (Exception e)
        {
            return -99;
        }
    }

}