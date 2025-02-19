using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasySaveWPFApp.Utilities
{
    // Utility class containing helper methods for file path validation.
    public class Utilities
    {
        public static bool IsValidPath(string path)
        {
            // Check if the path is null, empty, or consists only of whitespace.
            if (string.IsNullOrWhiteSpace(path))
            {
                return false;
            }

            try
            {
                // Check if the path is correctly formatted and is an absolute path.
                bool isRooted = Path.IsPathRooted(path);
                return isRooted;
            }
            catch
            {
                // If an exception occurs, return false (invalid path).
                return false;
            }
        }
    }
}
