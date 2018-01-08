using System.Text.RegularExpressions;
using HearthstoneDeckTracker.Model;
using HearthstoneDeckTracker.Utilities;

namespace HearthstoneDeckTracker.Tracker.HearthstoneLogHandlers
{
	public class PowerLogFileHandler
	{
		public void Handle(LogEntry entry, ref Game game)
		{
			if (LogEntryRegex.TagChangeRegex.IsMatch(entry.Line))
			{
				Match match = LogEntryRegex.TagChangeRegex.Match(entry.Line);
				string entity = match.Groups["entity"].Value;
				string tag = match.Groups["tag"].Value;
				string value = match.Groups["value"].Value;

				//finding names of players
				if (tag == "PLAYSTATE" && value == "PLAYING" && !int.TryParse(entity, out int x))
				{
					if (entity == Config.HearthstoneUsername())
					{
					    game.User.Name = entity;
					    Log.Info($"Detected username is '{entity}'.");
                    }
					else
					{
					    game.Opponent.Name = entity;
                        Log.Info($"Detected opponent is {entity}");
                    }
				}

				//finding who went first
				if (tag == "FIRST_PLAYER" && !game.User.Coin && !game.Opponent.Coin)
				{
					if (entity == game.User.Name)
					{
						game.User.Coin = true;
					    Log.Info("Detected user is going first (has the coin).");
                    }
					else
					{
						game.Opponent.Coin = true;
					    Log.Info("Detected opponent is going first (has the coin).'");
                    }
				}
			}
		}
	}
}
