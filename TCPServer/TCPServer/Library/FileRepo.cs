using System;
using System.IO;

namespace TCPServer.Library
{
    public class FileRepo
    {
        private const string LOG_NAME = "numbers.log";
        private string rootPath = string.Empty;
        public FileRepo()
        {
            rootPath = Path.Combine($"{AppDomain.CurrentDomain.BaseDirectory}\\..\\..\\log\\", LOG_NAME);
        }

        // do you ever need to read it? we're keeping the collection in memory.
        //public List<int> Read() { }

        public void Append(string item)
        {
            using (StreamWriter file = File.AppendText(rootPath))
            {
                file.WriteLine(item);
            }

            NumberCollectionSingleton.Instance.Add(Convert.ToInt32(item));
        }

        public void Clear()
        {
            File.WriteAllText(rootPath, string.Empty);
        }
        
        public bool LogFileExists()
        {
            return File.Exists(rootPath);
        }
    }
}
