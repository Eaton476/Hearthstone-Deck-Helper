using System;
using HearthDb.Deckstrings;

namespace HearthstoneDeckTracker.Model
{
	public class Game
	{
		public Player User { get; set; } = new Player
		{
			Deck = new Deck(),
			Coin = false,
			Health = 30,
			Id = 1,
			Name = "",
			Win = false
		};
		public Player Opponent { get; set; } = new Player
		{
			Deck = new Deck(),
			Coin = false,
			Health = 30,
			Id = 2,
			Name = "",
			Win = false
		};
	    internal DateTime TimeGameStart { get; set; }
		internal DateTime TimeGameFinish { get; set; }
		public TimeSpan Duration => TimeGameFinish - TimeGameStart;
		public string Result { get; set; }
		private int Turns { get; set; }
	    public bool GameStarted { get; set; } = false;

		public void StartGame()
		{
			TimeGameStart = DateTime.UtcNow;
		    GameStarted = true;
		}

		public void EndGame()
		{
			TimeGameFinish = DateTime.UtcNow;
		    GameStarted = false;
		}
	}
}
