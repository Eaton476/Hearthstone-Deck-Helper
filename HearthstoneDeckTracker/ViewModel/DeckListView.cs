using HearthDb.Deckstrings;
using HearthDb.Enums;

namespace HearthstoneDeckTracker.ViewModel
{
    public class DeckListView
    {
        public string Name { get; set; }
        public string Class { get; set; }
        public string Format { get; set; }
        public int DeckSize { get; set; }

        public DeckListView(Deck deck)
        {
            Name = deck.Name;
            Class = deck.GetHero().Name;
            Format = (deck.Format == FormatType.FT_STANDARD) ? "Standard" : "Wild";
            DeckSize = 0;

            foreach (var card in deck.GetCards())
            {
                DeckSize += card.Value;
            }
        }

        public DeckListView()
        {
        }
    }
}
