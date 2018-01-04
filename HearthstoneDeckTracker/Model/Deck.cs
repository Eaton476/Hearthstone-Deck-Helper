using System.Collections.Generic;
using HearthstoneDeckTracker.Utilities;

namespace HearthstoneDeckTracker.Model
{
    public class Deck
    {
        private const int InitialDeckSizeLimit = 30;
        public List<Card> Cards { get; set; }
		public string Name { get; set; }

	    public void AddCardInitial(Card card)
	    {
		    if (Cards.Count < InitialDeckSizeLimit)
		    {
			    Cards.Add(card);
		    }
		    else
		    {
			    Log.Error($"Unable to add card {card.Id} to deck '{Name}'");
		    }
	    }

	    public void AddCardInGame(Card card)
		{
		    Cards.Add(card);
	    }
    }
}
