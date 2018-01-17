using HearthDb;
using HearthDb.Enums;

namespace HearthstoneDeckTracker.ViewModel
{
    public class CardListViewDeck
    {
        public string Name { get; set; }
        public string Cost { get; set; }
        public int Quantity { get; set; }

        public CardListViewDeck(Card card, Locale language = Locale.enUS)
        {
            Name = card.GetLocName(language);
            Cost = card.Cost.ToString();
            Quantity = 1;
        }

        public CardListViewDeck()
        {
          
        }
    }
}
