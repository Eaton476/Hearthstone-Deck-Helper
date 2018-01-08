using HearthstoneDeckTracker.Model;
using HearthstoneDeckTracker.Utilities;

namespace HearthstoneDeckTracker.Tracker.HearthstoneLogHandlers
{
	public class LoadingScreenLogHandler
	{
	    public void Handle(LogEntry entry, ref Game game)
	    {
		    if (entry.Line.Contains("Gameplay.Start()"))
		    {
			    game.StartGame();
		        Log.Info("Detected that game is starting.");
		    }
			else if (entry.Line.Contains("Gameplay.OnDestroy()"))
		    {
		        if (game.GameStarted)
		        {
		            game.EndGame();
		            Log.Info("Detected that game has ended.");
		        }
		    }
	    }
    }
}
