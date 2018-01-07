using System;

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

		public void StartGame()
		{
			TimeGameStart = DateTime.UtcNow;
		}

		public void EndGame()
		{
			TimeGameFinish = DateTime.UtcNow;
		}
	}
}
