using System;
using System.Globalization;
using System.IO;

namespace HearthstoneDeckTracker.Utilities
{
    public static class Log
    {
        public enum Type
        {
            Info,
            Error,
            Debug
        }

        public static void Write(string message, Type level)
        {
            string date = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            string output = "";

            switch (level)
            {
                case Type.Info:
                    output = $"{date} | INFO: {message}{System.Environment.NewLine}";
                    break;
                case Type.Debug:
                    output = $"{date} | DEBUG: {message}{System.Environment.NewLine}";
                    break;
            }

            WriteToFile(output);
            
        }

        public static void Write(Exception e)
        {
            string date = DateTime.Now.ToString(CultureInfo.InvariantCulture);
            string output = $"{date} | ERROR: {e.StackTrace}";

            WriteToFile(output);
        }

        private static void WriteToFile(string message)
        {
            string date = $"{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}";
            string filename = $"{System.IO.Directory.GetCurrentDirectory()}\\{date}_Log.txt";

            File.AppendAllText(filename, message);
        }
    }
}
