using HearthDb.CardDefs;
using HearthDb.Deckstrings;
using HearthstoneDeckTracker.Enums;

namespace HearthstoneDeckTracker.Model
{
	public class Player
	{
		public int EntityId { get; set; }
        public int HeroEntityId { get; set; }
		public string Name { get; set; }
		public bool Coin { get; set; } = false;
		public string Result { get; set; }
		public Deck Deck { get; set; }
		public int ManaUsedThisGame { get; set; }
        public int MinionsDiedThisGame { get; set; }
        public int NumberOfHeroPowerUsesThisGame { get; set; }

	    public Player()
	    {
	        Deck = new Deck();
	    }
	}
}
