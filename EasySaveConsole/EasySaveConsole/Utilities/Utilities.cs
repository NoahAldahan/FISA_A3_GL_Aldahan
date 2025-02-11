using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveConsole.Utilities
{
    public class Utilities
    {
        public static bool IsValidPath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            try
            {
                // Vérifie si le chemin est valide et bien formé
                bool isRooted = Path.IsPathRooted(path);
                return isRooted;
            }
            catch
            {
                return false; // Si une exception est levée, le chemin est invalide
            }
        }
    }
}
