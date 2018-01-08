using System;

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
