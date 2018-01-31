using System;
using System.Collections.Generic;
using HearthDb.CardDefs;
using HearthDb.Deckstrings;

namespace HearthstoneDeckTracker.Model
{
	public class Game
	{
		public Player User { get; set; }
		public Player Opponent { get; set; }
	    internal DateTime TimeGameStart { get; set; }
		internal DateTime TimeGameFinish { get; set; }
		public TimeSpan Duration => TimeGameFinish - TimeGameStart;
		public string Result { get; set; }
		private int Turns { get; set; }
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
	}
}
