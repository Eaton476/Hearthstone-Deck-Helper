using HearthstoneDeckTracker.Enums;

namespace HearthstoneDeckTracker.Model
{
    public class Card
    {
		public int Id { get; set; }
		public string Name { get; set; }
		public CardType Type { get; set; }
		public Class Class { get; set; }
        public CardSet Set { get; set; }
        public CardRarity Rarity { get; set; }
        public int Mana { get; set; }
        public int Attack { get; set; }
        public int Health { get; set; }
    }
}
