using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Log
{
    public static class AsyncFileManager
    {
        private static int MaxRetries = 100;
        private static int RetryDelay = 1000;
        private static readonly object WriteFileLock = new object();
        private static readonly object ReadFileLock = new object();
        public static string LockedReadAllText(string filePath)
        {
            lock (ReadFileLock) // Ensures only one thread in this app reads at a time
            {
                for (int i = 0; i < MaxRetries; i++)
                {
                    try
                    {
                        // Read the file content
                        return File.ReadAllText(filePath);
                    }
                    catch (IOException)
                    {
                        // If the file is locked by another process, wait and retry
                        Thread.Sleep(RetryDelay);
                    }
                }

                Trace.WriteLine("Exception LockedReadAllText");
                throw new IOException($"Could not read the file '{filePath}' after multiple attempts.");
            }
        }


        public static void LockedWriteAllText(string filePath, string content)
        {

            lock (WriteFileLock) // Prevents multiple threads from writing at the same time
            {
                for (int i = 0; i < MaxRetries; i++)
                {
                    try
                    {
                        File.WriteAllText(filePath, content);
                        return; // Success, exit method
                    }
                    catch (IOException)
                    {
                        Thread.Sleep(RetryDelay); // Wait before retrying
                    }
                }

                Trace.WriteLine("Exception LockedWriteAllText");
                throw new IOException($"Could not write to the file '{filePath}' after multiple attempts.");
            }
        }
    }
}
