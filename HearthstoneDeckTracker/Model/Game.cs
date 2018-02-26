using System;
using System.Collections.Generic;
using HearthDb.CardDefs;
using HearthDb.Deckstrings;
using HearthstoneDeckTracker.Utilities;

namespace HearthstoneDeckTracker.Model
{
	public class Game
	{
		public Player User { get; set; } = new Player();
		public Player Opponent { get; set; } = new Player();
	    internal DateTime TimeGameStart { get; set; }
		internal DateTime TimeGameFinish { get; set; }
		public TimeSpan Duration => TimeGameFinish - TimeGameStart;
		public string Result { get; set; }
	    public bool GameInProgress { get; set; }
		public Dictionary<int, Entity> Entities { get; set; } = new Dictionary<int, Entity>();
        public int CurrentEntityId { get; set; }

		public void StartGame()
		{
			TimeGameStart = DateTime.UtcNow;
		    GameInProgress = true;
		}

		public void EndGame()
		{
			TimeGameFinish = DateTime.UtcNow;
		    GameInProgress = false;
		}

	    public void OutputEntitiesToLog()
	    {
	        foreach (var entity in Entities)
	        {
	            Log.Debug(entity.ToString());
	        }
	    }

		public void SaveGame()
		{
			
		}
	}
}
