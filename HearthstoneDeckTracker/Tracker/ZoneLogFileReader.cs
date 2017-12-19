using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using HearthstoneDeckTracker.Model;
using HearthstoneDeckTracker.Utilities;

namespace HearthstoneDeckTracker.Tracker
{
    public class ZoneLogFileReader
    {
        private FileSystemWatcher _watcher;
        private readonly string _logFile;
        private readonly string _directory;
        private long _lastPosition = 0;
        public List<Interaction> Interactions;

        public ZoneLogFileReader(string directory, string logFile)
        {
            _directory = directory;
            _logFile = logFile;
        }

        public void WatchLogFile()
        {
            _watcher = new FileSystemWatcher();
            _watcher.Path = _directory;
            _watcher.Filter = _logFile;
            _watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.Size;
            _watcher.Changed += OnChanged;
            _watcher.EnableRaisingEvents = true;
            Interactions = new List<Interaction>();
            Log.Write($"Starting watching {_logFile} for changes.", Log.Type.Info);
        }

        public void StartWatching()
        {
            _watcher.EnableRaisingEvents = true;
        }

        public void StopWatching()
        {
            _watcher.EnableRaisingEvents = false;
        }

        void OnChanged(object source, FileSystemEventArgs e)
        {
            string newLine = null;
            using (var file = File.Open($"{_directory}\\{_logFile}", FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                if (_lastPosition != 0)
                {
                    file.Position = _lastPosition;
                }

                using (var reader = new StreamReader(file))
                {
                    while (!reader.EndOfStream)
                    {
                        newLine = reader.ReadLine();
                        _lastPosition = file.Position;
                    }
                }
            }

            if (newLine != null)
            {
                if (newLine.Contains("ZoneChangeList.ProcessChanges()"))
                {
                    //Check if you/opponent has played a card
                    string pattern = @"D \d{2}:\d{2}:\d{2}.\d{7} ZoneChangeList.ProcessChanges\(\) - id=\d* local=(True|False) \[entityName=(.*) id=\d* zone=(HAND|PLAY|GRAVEYARD) zonePos=(\d) cardId=(.*) player=(\d)\] zone from (OPPOSING|FRIENDLY) (HAND|PLAY|GRAVEYARD) -> (OPPOSING|FRIENDLY) (HAND|PLAY|GRAVEYARD)";
                    Regex regex = new Regex(pattern, RegexOptions.None);
                    Match result = regex.Match(newLine);

                    if (result.Success)
                    {
                        //MessageBox.Show("It works!");
                        Interaction interaction = new Interaction{
                            Local = Boolean.Parse(result.Groups[0].ToString()),
                            EntityName = result.Groups[1].ToString(),
                            Zone = result.Groups[2].ToString(),
                            ZonePos = result.Groups[3].ToString(),
                            CardId = result.Groups[4].ToString(),
                            Player = Int32.Parse(result.Groups[5].ToString()),
                            Action = $"{result.Groups[7]} -> {result.Groups[9]}"
                        };

                        Interactions.Add(interaction);
                    }
                }
            }
        }
    }
}
