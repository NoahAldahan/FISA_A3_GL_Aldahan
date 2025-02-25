using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
        private static readonly object SerializeLock = new object();
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

                throw new IOException($"Could not write to the file '{filePath}' after multiple attempts.");
            }
        }

        public static string LockedSerialize<T>(T obj)
        {
            Trace.WriteLine("before locked");
            lock (SerializeLock)
            {

                Trace.WriteLine("before json content");
                string jsonContent = "";
                Trace.WriteLine("before serialize");
                jsonContent = JsonSerializer.Serialize<T>(obj, new JsonSerializerOptions { WriteIndented = true });
                Trace.WriteLine("before return : serialize :" + jsonContent);
                return jsonContent;
            }
        }
        public static T LockedDeserialize<T>(string jsonContent)
        {
            lock (SerializeLock)
            {
                T obj = JsonSerializer.Deserialize<T>(jsonContent);
                return obj;
            }
        }
    }
}
