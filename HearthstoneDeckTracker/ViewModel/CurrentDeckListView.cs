using HearthDb.Deckstrings;
using HearthDb.Enums;

namespace HearthstoneDeckTracker.ViewModel
{
    class CurrentDeckListView
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public string Format { get; set; }
        public int DeckSize { get; set; }

        public CurrentDeckListView(Deck deck)
        {
            Name = deck.Name;
            Class = deck.GetHero().Name;
            Format = (deck.Format == FormatType.FT_STANDARD) ? "Standard" : "Wild";
            DeckSize = deck.GetCards().Count;
        }

        public CurrentDeckListView()
        {
        }
    }
}
