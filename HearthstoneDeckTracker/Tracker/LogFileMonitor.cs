using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using HearthstoneDeckTracker.Utilities;

namespace HearthstoneDeckTracker.Tracker
{
    public class LogFileMonitor
    {
	    public readonly LogFileMonitorSettings Settings;
        private string _filepath;
        private long _offset;
        private bool _running;
        private bool _stop;
        private DateTime _startingPoint;
        private ConcurrentQueue<LogEntry> _logs = new ConcurrentQueue<LogEntry>();
        private Thread _thread;

        private const int BufferSize = 4096;

        public event Action<string> OnLogFileFound;
        public event Action<string> OnLogLineIgnored;
        public event Action<Exception> OnLogLineError;

        public LogFileMonitor(LogFileMonitorSettings settings)
        {
            Settings = settings;
        }

        public void Begin(DateTime startingPoint)
        {
            if (!_running)
            {
                _filepath = Path.Combine(Config.HearthstoneLogDirectory(), Settings.Name + ".log");
                _startingPoint = startingPoint;
                if (File.Exists(_filepath))
                {
                    _stop = false;
                    _offset = 0;
                    _thread = new Thread(MonitorLogFile)
                    {
                        IsBackground = true
                    };
                    try
                    {
                        _thread.Start();
                    }
                    catch (Exception e)
                    {
                        OnLogLineError?.Invoke(e);
                    }
                }
                else
                {
                    FileNotFoundException exception = new FileNotFoundException("Unable to start monitoring specified logfile", _filepath);
                    OnLogLineError?.Invoke(exception);
                }
            }
        }

	    public async Task Stop()
	    {
		    _stop = true;
		    while (_running || _thread == null || _thread.ThreadState == ThreadState.Unstarted)
		    {
				await Task.Delay(50);
			}
			    
		    _logs = new ConcurrentQueue<LogEntry>();
		    await Task.Factory.StartNew(() => _thread?.Join());
	    }

	    public IEnumerable<LogEntry> Collect()
	    {
		    var count = _logs.Count;
		    for (var i = 0; i < count; i++)
		    {
			    if (_logs.TryDequeue(out var line))
			    {
				    yield return line;
			    }
		    }
	    }

		private void MonitorLogFile()
        {
            _running = true;
            if (File.Exists(_filepath))
            {
                FindInitialPosition();
                while (!_stop)
                {
                    using (var fs = new FileStream(_filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        fs.Seek(_offset, SeekOrigin.Begin);
                        if (fs.Length == _offset)
                        {
                            Thread.Sleep(Config.LogFileUpdateDelay());
                            continue;
                        }
                        using (var sr = new StreamReader(fs))
                        {
                            string line;
                            while (!sr.EndOfStream && (line = sr.ReadLine()) != null)
                            {
                                if (line.StartsWith("D "))
                                {
                                    var next = sr.Peek();
                                    if (!sr.EndOfStream && !(next == 'D' || next == 'W'))
                                        break;
                                    var logLine = new LogEntry(Settings.Name, line);
									if ((!Settings.HasFilters || (Settings.StartFilters?.Any(x => logLine.LineContent.StartsWith(x)) ?? false)
									     || (Settings.ContainingFilters?.Any(x => logLine.LineContent.Contains(x)) ?? false))
									    && logLine.Time >= _startingPoint)
									{
                                        //OnLogFileFound?.Invoke($"MONITOR '{Settings.Name} - Successfully queued {logLine.LineContent}");
                                        _logs.Enqueue(logLine);
                                    }
                                    else
                                    {
                                        //OnLogLineIgnored?.Invoke($"MONITOR '{Settings.Name} - Didn't match filters {logLine.LineContent}");
                                    }
                                }
                                else
                                {
                                    //OnLogLineIgnored?.Invoke($"MONITOR '{Settings.Name} - Ignored {line}'");
                                }
                                _offset += Encoding.UTF8.GetByteCount(line + Environment.NewLine);
                            }
                        }
                    }

                    Thread.Sleep(Config.LogFileUpdateDelay());
                }
            }
            else
            {
                _running = false;
                FileNotFoundException exception = new FileNotFoundException("Unable to start monitoring specified logfile", _filepath);
                OnLogLineError?.Invoke(exception);
            }
        }

        private void FindInitialPosition()
        {
            if (File.Exists(_filepath))
            {
                using (var fs = new FileStream(_filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs, Encoding.ASCII))
                {
                    var offset = 0;
                    while (offset < fs.Length)
                    {
                        var sizeDiff = BufferSize - Math.Min(fs.Length - offset, BufferSize);
                        offset += BufferSize;
                        var buffer = new char[BufferSize];
                        fs.Seek(Math.Max(fs.Length - offset, 0), SeekOrigin.Begin);
                        sr.ReadBlock(buffer, 0, BufferSize);
                        var skip = 0;
                        for (var i = 0; i < BufferSize; i++)
                        {
                            skip++;
                            if (buffer[i] == '\n')
                                break;
                        }
                        offset -= skip;
                        var lines =
                            new string(buffer.Skip(skip).ToArray()).Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToArray();
                        for (var i = lines.Length - 1; i > 0; i--)
                        {
                            if (string.IsNullOrWhiteSpace(lines[i].Trim('\0')))
                                continue;
                            var logLine = new LogEntry(Settings.Name, lines[i]);
                            if (logLine.Time < _startingPoint)
                            {
                                var negativeOffset = lines.Take(i + 1).Sum(x => Encoding.UTF8.GetByteCount(x + Environment.NewLine));
                                _offset = Math.Max(fs.Length - offset + negativeOffset + sizeDiff, 0);
                                return;
                            }
                        }
                    }
                }
            }
            _offset = 0;
        }

        public DateTime FindEntryPoint(string logDirectory, string[] str)
        {
            var fileInfo = new FileInfo(Path.Combine(logDirectory, Settings.Name + ".log"));
            if (fileInfo.Exists)
            {
                var targets = str.Select(x => new string(x.Reverse().ToArray())).ToList();
                using (var fs = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs, Encoding.ASCII))
                {
                    var offset = 0;
                    while (offset < fs.Length)
                    {
                        offset += BufferSize;
                        var buffer = new char[BufferSize];
                        fs.Seek(Math.Max(fs.Length - offset, 0), SeekOrigin.Begin);
                        sr.ReadBlock(buffer, 0, BufferSize);
                        var skip = 0;
                        for (var i = 0; i < BufferSize; i++)
                        {
                            skip++;
                            if (buffer[i] == '\n')
                                break;
                        }
                        if (skip >= BufferSize)
                            continue;
                        offset -= skip;
                        var reverse = new string(buffer.Skip(skip).Reverse().ToArray());
                        var targetOffsets = targets.Select(x => reverse.IndexOf(x, StringComparison.Ordinal)).Where(x => x > -1).ToList();
                        var targetOffset = targetOffsets.Any() ? targetOffsets.Min() : -1;
                        if (targetOffset != -1)
                        {
                            var line = new string(reverse.Substring(targetOffset).TakeWhile(c => c != '\n').Reverse().ToArray());
                            return new LogEntry("", line).Time;
                        }
                    }
                }
            }
            return DateTime.MinValue;
        }
    }
}
