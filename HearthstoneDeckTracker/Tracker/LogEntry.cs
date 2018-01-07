using System;
using System.Text.RegularExpressions;
using HearthstoneDeckTracker.Utilities;

namespace HearthstoneDeckTracker.Tracker
{
    public class LogEntry
    {
	    public LogEntry(string logFile, string line)
	    {
		    LogFile = logFile;
		    Line = line;
		    Match match = LogEntryRegex.LogEntryStructure.Match(line);
		    if (match.Success)
		    {
			    DateTime time;
			    var ts = match.Groups["timestamp"].Value;
			    if (DateTime.TryParse(ts, out time))
			    {
				    Time = DateTime.Today.Add(time.TimeOfDay);
				    if (Time > DateTime.Now)
					    Time = Time.AddDays(-1);
			    }
			    LineContent = match.Groups["line"].Value;
		    }
	    }

	    public string LogFile { get; set; }
	    public DateTime Time { get; } = DateTime.Now;
	    public string Line { get; set; }
	    public string LineContent { get; set; }
	}
}
