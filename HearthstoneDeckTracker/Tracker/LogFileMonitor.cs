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
        private string _name;
        private string _filepath;
        private long _offset;
        private bool _running;
        private bool _stop;
        private bool _fileExists;
        private DateTime _startingPoint;
        private ConcurrentQueue<LogEntry> _logs;
        private Thread _thread;

        private const int BufferSize = 4096;

        public LogFileMonitor(string name, string filepath)
        {
            _name = name;
            _filepath = filepath;
        }

        public void Begin(DateTime startingPoint)
        {
            if (!_running)
            {
                _filepath = Path.Combine(Config.HearthstoneLogDirectory(), _name + ".log");
                _startingPoint = startingPoint;
                if (File.Exists(_filepath))
                {
                    _fileExists = true;
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
                        Log.Write(e);
                    }
                }
                else
                {
                    FileNotFoundException exception = new FileNotFoundException("Unable to start monitoring specified logfile", _filepath);
                    Log.Write(exception);
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
                                    var logLine = new LogEntry(_name, line);
                                    if (logLine.Time >= _startingPoint)
                                    {
                                        _logs.Enqueue(logLine);
                                    }
                                        
                                }
                                else
                                {
                                    Log.Write($"Log ignored - {_name} : {line}", Log.Type.Info);
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
                Log.Write(exception);
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
                            var logLine = new LogEntry(_name, lines[i]);
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
    }
}
