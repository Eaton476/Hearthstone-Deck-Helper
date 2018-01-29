using System.Collections.Generic;
using System.Text.RegularExpressions;
using HearthDb;
using HearthDb.CardDefs;
using HearthDb.Enums;
using HearthstoneDeckTracker.Model;
using HearthstoneDeckTracker.Utilities;
using HearthstoneDeckTracker.Utilities.Converters;

namespace HearthstoneDeckTracker.Tracker.HearthstoneLogHandlers
{
	public class PowerLogFileHandler
	{
        List<Entity> _tempEntities = new List<Entity>();

		public void Handle(LogEntry entry, ref Game game)
		{
			//Log.Debug($"Power is currently handling {entry.Line}");

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
					    Log.Info($"Detected username is {entity}.");
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
						game.User.Coin = false;
					    game.Opponent.Coin = true;
					    Log.Info("Detected opponent has the coin");
                    }
					else
					{
						game.Opponent.Coin = false;
					    game.User.Coin = true;
					    Log.Info("Detected user has the coin'");
                    }
				}
			}
			else if (LogEntryRegex.HeroRegex.IsMatch(entry.Line))
			{
				Match match = LogEntryRegex.HeroRegex.Match(entry.Line);
				string cardId = match.Groups["cardId"].Value;
				string player = match.Groups["player"].Value;

			    if (cardId != string.Empty)
			    {
					//its a hero card
				    if (cardId.Contains("HERO"))
				    {
					    Card hero = Cards.GetCardFromId(cardId);

					    if (game.User.Id.ToString() == player)
					    {
						    game.User.Deck.HeroDbfId = hero.DbfId;
						    Log.Info($"Detected that user is using {hero.Name}");
						}
					    else if (game.Opponent.Id.ToString() == player)
					    {
						    game.Opponent.Deck.HeroDbfId = hero.DbfId;
							Log.Info($"Detected that opponent is using {hero.Name}");
						}
				    }
			    }
			}
            else if (LogEntryRegex.CreationRegex.IsMatch(entry.Line))
			{
			    Match match = LogEntryRegex.CreationRegex.Match(entry.Line);
			    int id = int.Parse(match.Groups["id"].Value);
			    string cardId = match.Groups["cardId"].Value;
			    var zone = GameTagConverter.ParseEnum<Zone>(match.Groups["zone"].Value);
				if (!Database.Game.Entities.ContainsKey(id))
				{
					if (string.IsNullOrWhiteSpace(cardId) && zone != Zone.SETASIDE)
					{
						
					}
				}
			}
		}
	}
}
