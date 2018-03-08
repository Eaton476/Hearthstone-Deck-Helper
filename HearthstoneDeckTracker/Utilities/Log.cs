using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using HearthstoneDeckTracker.Enums;

namespace HearthstoneDeckTracker.Utilities
{
    public class Log
    {
		private static readonly Queue<string> LogQueue = new Queue<string>();

		public static bool Initialized { get; set; }
	    private static string _prevLine;
	    private static int _duplicateCount;

		internal static void Initialize()
	    {
		    if (!Initialized)
		    {
			    Trace.AutoFlush = true;
			    string date = $"{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}";
			    string logFile = Path.Combine(Config.LogFolder(), $"{date}.log");
			    if (!Directory.Exists(Config.LogFolder()))
			    {
				    Directory.CreateDirectory(Config.LogFolder());
			    }
			    else
			    {
				    try
				    {
						FileInfo file = new FileInfo(logFile);
					    if (file.Exists)
					    {
						    using (FileStream fs = new FileStream(logFile, FileMode.Open, FileAccess.Read, FileShare.None))
						    {
								//reserves access to the file so no other application can access.
						    }
					    }
					    else
					    {
						    File.Create(logFile).Dispose();
					    }
				    }
				    catch (Exception)
				    {
						MessageBox.Show("Another instance of Hearthstone Deck Tracker is already running.",
						    "Error starting Hearthstone Deck Tracker", MessageBoxButton.OK, MessageBoxImage.Error);
					    Application.Current.Shutdown();
				    }
			    }
			    try
			    {
				    Trace.Listeners.Add(new TextWriterTraceListener(new StreamWriter(logFile, false)));
			    }
			    catch (Exception e)
			    {
				    MessageBox.Show($"Unable to access log file {e.StackTrace}");
			    }

			    Initialized = true;
			    foreach (var log in LogQueue)
			    {
				    Trace.WriteLine(log);
			    }
		    }
	    }

	    public static void WriteLine(string message, LogType type, [CallerMemberName] string member = "", [CallerFilePath] string sourceFilePath = "")
	    {
			string file = sourceFilePath?.Split('/', '\\').LastOrDefault()?.Split('.').FirstOrDefault();
		    string line = $"{type}|{file}.{member} >> {message}";

		    if (line == _prevLine)
		    {
			    _duplicateCount++;
		    }
		    else
		    {
			    if (_duplicateCount > 0)
			    {
				    Write($"... {_duplicateCount} duplicate messages");
			    }
			    _prevLine = line;
			    _duplicateCount = 0;
				Write(line);
		    }
		}

	    public static void Write(string line)
	    {
			line = $"{DateTime.Now.ToLongTimeString()}|{line}";
		    if (Initialized)
		    {
			    Trace.WriteLine(line);
		    }
		    else
		    {
			    LogQueue.Enqueue(line);
		    }
		}

	    public static void Debug(string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
		    => WriteLine(msg, LogType.Debug, memberName, sourceFilePath);

	    public static void Info(string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
		    => WriteLine(msg, LogType.Info, memberName, sourceFilePath);

	    public static void Warn(string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
		    => WriteLine(msg, LogType.Warning, memberName, sourceFilePath);

	    public static void Error(string msg, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
		    => WriteLine(msg, LogType.Error, memberName, sourceFilePath);

	    public static void Error(Exception ex, [CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "")
		    => WriteLine(ex.ToString(), LogType.Error, memberName, sourceFilePath);

		//      public enum Type
		//      {
		//          Info,
		//          Error,
		//          Debug
		//      }

		//      public static void Write(string message, Type level)
		//      {
		//          string date = DateTime.Now.ToString(CultureInfo.InvariantCulture);
		//          string output = "";

		//          switch (level)
		//          {
		//              case Type.Info:
		//                  output = $"{date} | INFO: {message}{System.Environment.NewLine}";
		//                  break;
		//              case Type.Debug:
		//                  output = $"{date} | DEBUG: {message}{System.Environment.NewLine}";
		//                  break;
		//          }

		//          WriteToFile(output);

		//      }

		//      public static void Write(Exception e)
		//      {
		//          string date = DateTime.Now.ToString(CultureInfo.InvariantCulture);
		//          string output = $"{date} | ERROR: {e.StackTrace}{Environment.NewLine}";

		//          WriteToFile(output);
		//      }

		//   public static void Write(LogEntry entry)
		//   {
		//	string date = DateTime.Now.ToString(CultureInfo.InvariantCulture);
		//    string output = $"{date} | Processed entry {entry.LineContent} at {entry.Time.ToShortTimeString()}{Environment.NewLine}";

		//	WriteToFile(output);
		//}

		//      private static void WriteToFile(string message)
		//      {
		//          string date = $"{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}";
		//          string filename = $"{Directory.GetCurrentDirectory()}\\{date}_Log.txt";

		//          File.AppendAllText(filename, message);
		//      }
	}
}
