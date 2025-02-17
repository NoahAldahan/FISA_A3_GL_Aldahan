using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoSoftLibrary
{
    public class CryptoSoftLibrary
    {
        public static int EncryptFile(string path, string key)
        {
            if (path == null || key == null || path == "" || key == "" )
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
}
