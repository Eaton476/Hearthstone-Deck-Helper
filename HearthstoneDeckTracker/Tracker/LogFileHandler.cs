using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using HearthstoneDeckTracker.Model;
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
        private bool _running = false;

        private void OnLogFileFound(string msg) => Log.Info(msg);
        private void OnLogLineIgnored(string msg) => Log.Info(msg);
        private void OnLogLineError(Exception ex) => Log.Error(ex);

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
            _running = true;
            _logFileController.Start();
            Log.Info("Starting monitoring Hearthstone log files");
        }

        public async void StopAsync()
        {
            bool result = false;

            if (!_stop)
            {
                Task<bool> promise = _logFileController.Stop();
                _stop = await promise;
                if (!_stop)
                {
                    Log.Error("Unable to stop monitoring");
                }
                else
                {
                    _running = false;
                }
            }
        }

        private void HandleLine(List<LogEntry> entries)
        {
            foreach (var entry in entries)
            {
                if (!_stop)
                {
	                //Log.Info($"Handling line - {entry.LineContent}");

	                switch (entry.LogFile)
	                {
		                case "FullScreenFX":
			                break;
		                case "LoadingScreen":
							_loadingScreenLogHandler.Handle(entry);
			                break;
		                case "Power":
							_powerLogFileHandler.Handle(entry);
			                break;
	                }
                }
            }   
        }
        public bool GetRunning()
        {
            return _running;
        }
    }
}
