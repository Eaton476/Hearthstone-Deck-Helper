using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Xml.Serialization;
using HearthDb;
using HearthDb.CardDefs;
using HearthDb.Deckstrings;
using HearthDb.Enums;
using HearthstoneDeckTracker.Utilities;
using HearthstoneDeckTracker.Utilities.Converters;
using HearthstoneDeckTracker.ViewModel;
using LiveCharts;
using LiveCharts.Wpf;

namespace HearthstoneDeckTracker.Model
{
    public static class Database
    {
        public static List<Deck> CurrentDecks { get; set; } = new List<Deck>();
        public static Game CurrentGame { get; set; } = new Game();
        public static List<Game> RecordedGames { get; set; } = new List<Game>();

        static Database()
        {
            //AddTestDeck();
        }

        public static void StartGame()
        {
            CurrentGame.StartGame();
        }

        public static void EndGame()
        {
            CurrentGame.EndGame();
            RecordedGames.Add(CurrentGame);
            CurrentGame = new Game();
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
            SavePlayerCreatedDecksToFile();
            SaveRecordedGamesToFile();
        }

        private static void SavePlayerCreatedDecksToFile()
        {
            List<string> deckStrings = CurrentDecks.Select(x => DeckSerializer.Serialize(x, true)).ToList();
            string path = Path.Combine(Config.SavedDataFolder(), Config.SavedDecksFile());
            File.WriteAllLines(path, deckStrings);

            Log.Info("Successfully saved player created decks to file.");
        }

        private static void SaveRecordedGamesToFile()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Game>));
            string path = Config.RecordedGamesXmlFile();
            using (TextWriter writer = new StreamWriter(path))
            {
                xmlSerializer.Serialize(writer, RecordedGames);
            }

            Log.Info("Successfully saved the recorded games entities to disk.");
        }

        public static void LoadData()
        {
            LoadPlayerCreatedDecksFromFile();
            LoadRecordedGamesFromFile();
        }

        private static void LoadPlayerCreatedDecksFromFile()
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

            Log.Info("Successfully loaded player created decks from file.");
        }

        private static void LoadRecordedGamesFromFile()
        {
            XmlSerializer xmlDeSerializer = new XmlSerializer(typeof(List<Game>));
            string path = Config.RecordedGamesXmlFile();
            if (File.Exists(path))
            {
                using (TextReader reader = new StreamReader(path))
                {
                    RecordedGames = (List<Game>) xmlDeSerializer.Deserialize(reader);
                    reader.Close();
                }

                Log.Info("Successfully loaded recorded game data from disk.");
            }
            else
            {
                Log.Warn($"Unable to find recorded data at '{path}'");
            }
        }

        public static SeriesCollection GetGameResultSeries()
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            SeriesCollection seriesCollection = new SeriesCollection();
            IEnumerable<IGrouping<string, Game>> results = RecordedGames.GroupBy(x => x.User.Result);
            Func<ChartPoint, string> labelPoint = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            foreach (var result in results) 
            {
                if (result.Key != null)
                {
                    string resultName = result.Key;
                    int value = result.Count();
                    PieSeries series = new PieSeries
                    {
                        Title = textInfo.ToTitleCase(resultName.ToLower()),
                        Values = new ChartValues<int>{value},
                        DataLabels = true,
                        LabelPoint = labelPoint
                    };

                    seriesCollection.Add(series);
                }
            }
            
            return seriesCollection;
        }
    }
}
