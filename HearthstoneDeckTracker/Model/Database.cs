using System.Collections.Generic;
using System.Threading.Tasks;
using HearthDb;
using HearthDb.Deckstrings;
using HearthDb.Enums;

namespace HearthstoneDeckTracker.Model
{
    public static class Database
    {
        public static List<Deck> CurrentDecks { get; set; } = new List<Deck>();

        static Database()
        {
            Deck testDeck = new Deck();
            testDeck.Name = "TESTDECK";
            CurrentDecks.Add(testDeck);
        }
    }
}
