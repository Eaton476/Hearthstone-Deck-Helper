using System.IO;

namespace HearthstoneDeckTracker.Interfaces
{
    interface IWatchable
    {
        void WatchLogFile();
        void StartWatching();
        void StopWatching();
    }
}
