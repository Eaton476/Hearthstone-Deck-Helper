using HearthstoneDeckTracker.Model;
using HearthstoneDeckTracker.Utilities;

namespace HearthstoneDeckTracker.Tracker.HearthstoneLogHandlers
{
	public class LoadingScreenLogHandler
	{
	    public void Handle(LogEntry entry)
	    {
		    if (entry.Line.Contains("Gameplay.Start()"))
		    {
			    Database.StartGame();
		        Log.Info("Detected that game is starting.");
		    }
			else if (entry.Line.Contains("Gameplay.OnDestroy()"))
		    {
		        if (Database.CurrentGame.GameInProgress)
		        {
			        Database.EndGame();
		            Log.Info("Detected that game has ended.");
		        }
		    }
	    }
    }
}
