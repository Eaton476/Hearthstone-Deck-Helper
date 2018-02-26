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
		public bool Win { get; set; }
		public int Health { get; set; }
		public Deck Deck { get; set; }
		public bool HasName => Name == null;
	}
}
