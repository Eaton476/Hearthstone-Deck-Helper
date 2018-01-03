using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HearthstoneDeckTracker.Tracker.HearthstoneLogHandlers;
using HearthstoneDeckTracker.Utilities;

namespace HearthstoneDeckTracker.Tracker
{
    public class LogFileHandler
    {
        private readonly FullScreenFXLogHandler _fullScreenFxLogHandler = new FullScreenFXLogHandler();
        private readonly LoadingScreenLogHandler _loadingScreenLogHandler = new LoadingScreenLogHandler();
        private readonly PowerLogFileHandler _powerLogFileHandler = new PowerLogFileHandler();
        private readonly LogFileController _logFileController;
        private bool _stop;

        private void OnLogFileFound(string msg) => Log.Write(msg, Log.Type.Info);
        private void OnLogLineIgnored(string msg) => Log.Write(msg, Log.Type.Info);
        private void OnLogLineError(Exception ex) => Log.Write(ex);

        public LogFileHandler()
        {
            LogFileMonitorSettings fullScreenSettings = new LogFileMonitorSettings
            {
                Name = "FullScreenFX",
                Reset = false
            };
            LogFileMonitorSettings loadingScreenSettings = new LogFileMonitorSettings
            {
                Name = "LoadingScreen",
                StartFilters = new[] { "LoadingScreen.OnSceneLoaded", "Gameplay" }
            };
            LogFileMonitorSettings powerSettings = new LogFileMonitorSettings
            {
                Name = "Power",
                StartFilters = new[] { "PowerTaskList.DebugPrintPower", "GameState." },
                ContainingFilters = new[] { "Begin Spectating", "Start Spectator", "End Spectator" }
            };

            _logFileController = new LogFileController(new []
            {
	            fullScreenSettings,
                loadingScreenSettings,
                powerSettings
            });
            _logFileController.OnNewLines += HandleLine;
            _logFileController.OnLogFileFound += OnLogFileFound;
            _logFileController.OnLogLineIgnored += OnLogLineIgnored;
            _logFileController.OnLogLineError += OnLogLineError;
        }

        public void Start()
        {
            _stop = false;
            _logFileController.Start();
            Log.Write("Starting monitoring Hearthstone log files", Log.Type.Info);
        }

        private void HandleLine(List<LogEntry> entries)
        {
            foreach (var entry in entries)
            {
                if (!_stop)
                {
	                Log.Write(entry);

	                switch (entry.LogFile)
	                {
		                case "FullScreenFX":
			                break;
		                case "LoadingScreen":
			                break;
		                case "Power":
			                break;
	                }
                }
            }   
        }
    }
}
