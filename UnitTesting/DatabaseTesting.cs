using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HearthDb;
using HearthDb.Deckstrings;
using HearthDb.Enums;
using HearthstoneDeckTracker.Model;
using HearthstoneDeckTracker.Utilities;
using HearthstoneDeckTracker.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class DatabaseTesting
    {
        [TestInitialize]
        public void Initialise()
        {
            Database.LoadData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Database.ClearLoadedData();
        }

        [TestMethod]
        public void Database_CreateNewDeck_Success()
        {
            Card card = Cards.GetCardFromId("HERO_05");
            Database.CreateNewDeck(card, "TEST");

            Assert.IsTrue(Database.CurrentDecks.Exists(x => x.Name == "TEST"));
        }

        [TestMethod]
        public void Database_EditDeck_Success()
        {
            string existingDeckName = Database.CurrentDecks.First().Name;
            string newName = "TEST";
            Card card = Cards.GetCardFromId("HERO_05");

            Database.EditDeck(existingDeckName, newName, card);

            Assert.IsTrue(Database.CurrentDecks.Exists(x => x.Name == "TEST"));
        }

        [TestMethod]
        public void Database_DeleteDeck_Success()
        {
            string existingDeckName = Database.CurrentDecks.First().Name;

            Database.DeleteDeck(existingDeckName);

            Assert.IsFalse(Database.CurrentDecks.Exists(x => x.Name == existingDeckName));
        }

        [TestMethod]
        public void Database_GetCardListViewDeckFromDeck_Success()
        {
            Deck deck = Database.CurrentDecks.First();

            List<CardListViewDeck> cards = Database.GetCardListViewDeckFromDeck(deck);

            Assert.IsTrue(cards.Count > 0);
        }

        [TestMethod]
        public void Database_SavePlayerCreatedDecksToFile_Success()
        {
            string path = Path.Combine(Config.SavedDataFolder(), Config.SavedDecksFile());

            Database.SavePlayerCreatedDecksToFile();

            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void Database_LoadPlayerCreatedDecksFromFile_Success()
        {
            Database.CurrentDecks.Clear();

            Database.LoadPlayerCreatedDecksFromFile();

            Assert.IsTrue(Database.CurrentDecks.Any());
        }

        [TestMethod]
        public void Database_SaveRecordedGamesToFile_Success()
        {
            string path = Path.Combine(Config.SavedDataFolder(), Config.RecordedGamesXmlFile());

            Database.SaveRecordedGamesToFile();

            Assert.IsTrue(File.Exists(path));
        }

        [TestMethod]
        public void Database_LoadRecordGamesFromFile_Success()
        {
            Database.RecordedGames.Clear();

            Database.LoadRecordedGamesFromFile();

            Assert.IsTrue(Database.RecordedGames.Any());
        }

        [TestMethod]
        public void Database_GenerateCardSuggestions_Success()
        {
            Card card = Cards.GetCardFromId("AT_008");

            List<CardSuggestion> suggestions = Database.GenerateCardSuggestions(card);

            Assert.IsTrue(suggestions.Any());
        }
    }
}
