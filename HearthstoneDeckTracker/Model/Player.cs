using System.Xml.Serialization;
using HearthDb.CardDefs;
using HearthDb.Deckstrings;
using HearthstoneDeckTracker.Enums;

namespace HearthstoneDeckTracker.Model
{
	public class Player
	{
        [XmlAttribute]
		public int EntityId { get; set; }
	    [XmlAttribute]
        public string Name { get; set; }
	    [XmlElement]
        public bool Coin { get; set; } = false;
	    [XmlElement]
        public string Result { get; set; }
        [XmlElement]
		public Deck Deck { get; set; }
	    [XmlElement]
        public int ManaUsedThisGame { get; set; }
	    [XmlElement]
        public int MinionsDiedThisGame { get; set; }
	    [XmlElement]
        public int NumberOfHeroPowerUsesThisGame { get; set; }

	    public Player()
	    {
	        Deck = new Deck();
	    }
	}
}
