using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearthstoneDeckTracker.Utilities;

namespace HearthstoneDeckTracker.Tracker
{
	public class LogFileController
	{
		private bool _running;
		private bool _stop;
		private List<LogFileMonitor> _logFileMonitors = new List<LogFileMonitor>();

	    public event Action<List<LogEntry>> NewEntrys;

	    public LogFileController(IEnumerable<LogFileMonitorSettings> settings)
	    {
	        foreach (var setting in settings)
	        {
	            LogFileMonitor monitor = new LogFileMonitor(setting);
		        monitor.OnLogFileFound += (msg) => OnLogFileFound?.Invoke(msg);
		        monitor.OnLogLineIgnored += (msg) => OnLogLineIgnored?.Invoke(msg);
	            monitor.OnLogLineError += (ex) => OnLogLineError?.Invoke(ex);
				_logFileMonitors.Add(monitor);
	        }
	    }

	    private LogFileMonitor PowerLogWatcher => _logFileMonitors.Single(x => x.Settings.Name == "Power");
	    private LogFileMonitor LoadingScreenLogWatcher => _logFileMonitors.Single(x => x.Settings.Name == "LoadingScreen");

		public event Action<List<LogEntry>> OnNewLines;
		public event Action<string> OnLogFileFound;
		public event Action<string> OnLogLineIgnored;
	    public event Action<Exception> OnLogLineError;

        public async void Start()
	    {
	        if (!_running)
	        {
	            DateTime startingRow = GetStartingRow();
	            foreach (var logFileMonitor in _logFileMonitors)
	            {
	                logFileMonitor.Begin(startingRow);
	            }
		        _running = true;
		        _stop = false;
				var newLines = new SortedList<DateTime, List<LogEntry>>();
		        while (!_stop)
		        {
			        await Task.Factory.StartNew(() =>
			        {
				        foreach (var logFileMonitor in _logFileMonitors)
				        {
					        var lines = logFileMonitor.Collect();
					        foreach (var line in lines)
					        {
						        if (!newLines.TryGetValue(line.Time, out var logEntries))
						        {
							        newLines.Add(line.Time, logEntries = new List<LogEntry>());
						        }
								logEntries.Add(line);
					        }
				        }
			        });
		            var linesToHandle = new List<LogEntry>(newLines.Values.SelectMany(x => x));
		            OnNewLines?.Invoke(linesToHandle);
                    newLines.Clear();
		            await Task.Delay(Config.LogFileUpdateDelay());
		        }
	        }
	    }

		public async Task<bool> Stop(bool force = false)
		{
			if (_running)
			{
				_stop = true;
				while (_running)
				{
					await Task.Delay(50);
				}
				await Task.WhenAll(_logFileMonitors.Where(x => force || x.Settings.Reset).Select(x => x.Stop()));
				return true;
			}

			return false;
		}

		private DateTime GetStartingRow()
		{
		    DateTime startingPowerRow = PowerLogWatcher.FindEntryPoint(Config.HearthstoneLogDirectory(),
		        new[] {"tag=GOLD_REWARD_STATE", "End Spectator"});
		    DateTime startingLoadingScreenRow =
		        LoadingScreenLogWatcher.FindEntryPoint(Config.HearthstoneLogDirectory(), new[] {"Gameplay.Start"});

		    if (startingLoadingScreenRow > startingPowerRow)
		    {
		        return startingLoadingScreenRow;
		    }
			return startingPowerRow;
		}
	}
}
