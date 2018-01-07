using HearthstoneDeckTracker.Model;

namespace HearthstoneDeckTracker.Tracker.HearthstoneLogHandlers
{
	public class LoadingScreenLogHandler
	{
	    public void Handle(LogEntry entry, ref Game game)
	    {
		    if (entry.Line.Contains("Gameplay.Start()"))
		    {
			    game.StartGame();
		    }
			else if (entry.Line.Contains("Gameplay.OnDestroy()"))
		    {
			    game.EndGame();
		    }
	    }
    }
}
