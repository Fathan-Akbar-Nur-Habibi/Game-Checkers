using System;
using System.IO;

namespace GameCheckers
{
    public class Logger
    {
        private static Logger _instance;
        private static readonly object _lock = new object();
        private readonly StreamWriter _logFile;

        private Logger()
        {
            _logFile = new StreamWriter("game_log.txt", true);
        }

        public static Logger Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new Logger();
                }
            }
        }

        public void Log(string message)
        {
            string logMessage = $"{DateTime.Now}: {message}";
            Console.WriteLine(logMessage);
            _logFile.WriteLine(logMessage);
            _logFile.Flush();
        }

        public void Close()
        {
            _logFile.Close();
        }
    }
}