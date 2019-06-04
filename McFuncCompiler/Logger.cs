using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McFuncCompiler
{
    public static class Logger
    {
        public static void Log(Level level, string message)
        {
            string line = $"[{DateTime.Now.ToString()}] [{level.ToString()}]: {message}";

            Console.WriteLine(line);
        }

        public static void Debug(string message)
        {
            Log(Level.Debug, message);
        }

        public static void Info(string message)
        {
            Log(Level.Info, message);
        }

        public static void Warning(string message)
        {
            Log(Level.Warning, message);
        }

        public static void Error(string message)
        {
            Log(Level.Error, message);
        }

        public enum Level
        {
            Debug,
            Info,
            Warning,
            Error
        }
    }
}
