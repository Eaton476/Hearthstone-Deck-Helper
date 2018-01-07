using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
				if (tag == "PLAYSTATE" && value == "PLAYING")
				{
					if (game.User.HasName)
					{
						game.Opponent.Name = entity;
					}
					else
					{
						game.User.Name = entity;
					}
				}

				//finding who went first
				if (tag == "FIRST__PLAYER" && !game.User.Coin && !game.Opponent.Coin)
				{
					if (entity == game.User.Name)
					{
						game.User.Coin = true;
					}
					else
					{
						game.Opponent.Coin = true;
					}
				}
			}
		}
	}
}
