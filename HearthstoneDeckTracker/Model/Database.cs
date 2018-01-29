using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HearthDb;
using HearthDb.Deckstrings;
using HearthDb.Enums;
using HearthstoneDeckTracker.Utilities;
using HearthstoneDeckTracker.ViewModel;

namespace HearthstoneDeckTracker.Model
{
    public static class Database
    {
        public static List<Deck> CurrentDecks { get; set; } = new List<Deck>();
        public static Game Game { get; set; } = new Game();

        static Database()
        {
            //AddTestDeck();
        }

        private static void AddTestDeck()
        {
            Deck testDeck = new Deck();
            testDeck.Name = "TESTDECK";
            testDeck.HeroDbfId = 7;
            testDeck.Format = FormatType.FT_STANDARD;
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
            newDeck.Format = FormatType.FT_STANDARD;

            CurrentDecks.Add(newDeck);
        }

        public static void EditDeck(string oldName, string newName, Card heroCard)
        {
            Deck deckToEdit = CurrentDecks.First(x => x.Name == oldName);
            deckToEdit.Name = newName;
            deckToEdit.HeroDbfId = heroCard.DbfId;
        }

        public static void DeleteDeck(string deckName)
        {
            CurrentDecks.Remove(CurrentDecks.First(x => x.Name == deckName));
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

        public static void SaveData()
        {
            List<string> deckStrings = CurrentDecks.Select(x => DeckSerializer.Serialize(x, true)).ToList();
            string path = Path.Combine(Config.SavedDataFolder(), Config.SavedDecksFile());
            File.WriteAllLines(path, deckStrings);
            Log.Info("Successfully saved decks to file.");
        }

        public static void LoadData()
        {
            string path = Path.Combine(Config.SavedDataFolder(), Config.SavedDecksFile());
            List<string> lines = File.ReadAllLines(path).ToList();
            List<string> decksToLoad = new List<string>();
            string input = "";
            for (int i = 0; i < lines.Count; i++)
            {
                input = input + lines[i] + "\n";
                if (lines.Count == i + 1)
                {
                    decksToLoad.Add(input);
                    input = "";
                }
                else
                {
                    if (lines[i + 1].StartsWith("###"))
                    {
                        decksToLoad.Add(input);
                        input = "";
                    }
                }
            }
            decksToLoad.ForEach(x => CurrentDecks.Add(DeckSerializer.Deserialize(x)));

            Log.Info("Successfully loaded decks from file.");
        }
    }
}
