using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HearthDb;
using HearthDb.Deckstrings;
using HearthDb.Enums;
using HearthstoneDeckTracker.ViewModel;

namespace HearthstoneDeckTracker.Model
{
    public static class Database
    {
        public static List<Deck> CurrentDecks { get; set; } = new List<Deck>();

        static Database()
        {
            Deck testDeck = new Deck();
            testDeck.Name = "TESTDECK";
            testDeck.HeroDbfId = 7;
            testDeck.CardDbfIds.Add(2757, 2);
            testDeck.CardDbfIds.Add(2507, 2);
            testDeck.CardDbfIds.Add(1688, 2);
            CurrentDecks.Add(testDeck);
        }

        public static void CreateNewDeck(Card heroCard, string name)
        {
            Deck newDeck = new Deck();
            newDeck.HeroDbfId = heroCard.DbfId;
            newDeck.Name = name;

            CurrentDecks.Add(newDeck);
        }

        public static Deck GetDeckFromCurrentDeckListView(DeckListView listView)
        {
            return CurrentDecks.Find(x => x.Name == listView.Name);
        }

        public static List<CardListViewDeck> GetCardListViewDeckFromDeck(Deck deck)
        {
            List<CardListViewDeck> ret = new List<CardListViewDeck>();
            foreach (var card in deck.GetCards())
            {
                CardListViewDeck cardListView = new CardListViewDeck
                {
                    Name = card.Key.Name,
                    Cost = card.Key.Cost.ToString(),
                    Quantity = card.Value
                };

                ret.Add(cardListView);
            }

            return ret;
        }
    }
}
