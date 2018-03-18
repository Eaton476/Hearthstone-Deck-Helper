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
using LiveCharts.Defaults;
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

        public static SeriesCollection GetHeroSelectionSeries()
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            SeriesCollection seriesCollection = new SeriesCollection();
            IEnumerable<IGrouping<int, Game>> results = RecordedGames.GroupBy(x => x.User.Deck.HeroDbfId);

            foreach (var result in results)
            {
                Card heroCard = Cards.GetFromDbfId(result.Key, false);
                if (heroCard != null)
                {
                    PieSeries series = new PieSeries
                    {
                        Title = heroCard.Name,
                        Values = new ChartValues<ObservableValue> { new ObservableValue(result.Count()) },
                        DataLabels = true
                    };

                    seriesCollection.Add(series);
                }
            }

            return seriesCollection;
        }

        public static int GetAverageMinionDeaths()
        {
            int numberOfValues = 0;
            int average = 0;

            foreach (var game in RecordedGames)
            {
                numberOfValues++;
                average += game.User.MinionsDiedThisGame;
            }

            average = average / numberOfValues;
            return average;
        }

        class CostValue
        {
            public int Cost { get; set; }
            public int Occurance { get; set; }
        }

        public static SeriesCollection GetUserCardCostAverageSeries()
        {
            int totalCards = 0;
            SeriesCollection seriesCollection = new SeriesCollection();
            List<CostValue> costs = new List<CostValue>();
            for (int i = 0; i <= 10; i++)
            {
                costs.Add(new CostValue
                {
                    Cost = i,
                    Occurance = 0
                });
            }

            foreach (Game game in RecordedGames)
            {
                foreach (Card card in game.User.Deck.CardsInDeck)
                {
                    CostValue cost;

                    if (card.Cost < 10)
                    {
                        cost = costs.Find(x => x.Cost == card.Cost);
                        cost.Occurance += 1;
                    }
                    else
                    {
                        cost = costs.Find(x => x.Cost == card.Cost);
                        cost.Occurance += 1;
                    }

                    totalCards += 1;
                }                   
            }

            foreach (CostValue cost in costs)
            {
                string title = cost.Cost.ToString();
                int average = (int)Math.Round((double)(100 * cost.Occurance) / totalCards);
                if (cost.Cost == 10)
                {
                    title = title + "+";
                }

                ColumnSeries series = new ColumnSeries
                {
                    Title = title,
                    Values = new ChartValues<float> { average}
                };

                seriesCollection.Add(series);
            }

            return seriesCollection;
        }

        public static SeriesCollection GetOpponentCardCostAverageSeries()
        {
            int totalCards = 0;
            SeriesCollection seriesCollection = new SeriesCollection();
            List<CostValue> costs = new List<CostValue>();
            for (int i = 0; i <= 10; i++)
            {
                costs.Add(new CostValue
                {
                    Cost = i,
                    Occurance = 0
                });
            }

            foreach (Game game in RecordedGames)
            {
                foreach (Card card in game.Opponent.Deck.CardsInDeck)
                {
                    CostValue cost;

                    if (card.Cost < 10)
                    {
                        cost = costs.Find(x => x.Cost == card.Cost);
                        cost.Occurance += 1;
                    }
                    else
                    {
                        cost = costs.Find(x => x.Cost == card.Cost);
                        cost.Occurance += 1;
                    }

                    totalCards += 1;
                }
            }

            foreach (CostValue cost in costs)
            {
                string title = cost.Cost.ToString();
                int average = (int)Math.Round((double)(100 * cost.Occurance) / totalCards);
                if (cost.Cost == 10)
                {
                    title = title + "+";
                }

                ColumnSeries series = new ColumnSeries
                {
                    Title = title,
                    Values = new ChartValues<float> { average }
                };

                seriesCollection.Add(series);
            }

            return seriesCollection;
        }

        public static List<CardSuggestion> GenerateCardSuggestions(Card cardToEvaluate)
        {
            List<CardSuggestion> suggestions = new List<CardSuggestion>();

            foreach (Card card in Cards.Collectible.Values)
            {
                if (card.Type == cardToEvaluate.Type && (card.Class == cardToEvaluate.Class || card.Class == CardClass.NEUTRAL) && cardToEvaluate != card)
                {
                    int points = 0;
                    List<string> reasons = new List<string>();
                    int opponentCount = GetOpponentCardUsage(card);
                    int userCount = GetUserWinCardUsage(card);

                    points += opponentCount;
                    if (opponentCount > 0)
                    {
                        reasons.Add($"Your opponents have used this card {opponentCount} time(s) against you.");
                    }
                    else
                    {
                        reasons.Add("Unfortunately, we have no record of this card being used against you.");
                    }

                    points += userCount;
                    if (userCount > 0)
                    {
                        reasons.Add($"You have used this card {userCount} time(s) in games you have won previously.");
                    }
                    else
                    {
                        reasons.Add("Unfortunately, we have no record of you winning a game with this card so far.");
                    }

                    if (card.Cost == cardToEvaluate.Cost)
                    {
                        points += 20;
                        reasons.Add("Both cards have the same cost. (+20)");
                    }
                    else if (card.Cost < cardToEvaluate.Cost)
                    {
                        points += 25;
                        reasons.Add("This card is cheaper then the card you have chosen. (+25)");
                    }

                    if (card.Class == cardToEvaluate.Class)
                    {
                        points += 5;
                        reasons.Add("Both cards are in the same class. (+5)");
                    }

                    if (card.Type == CardType.MINION)
                    {
                        if (card.Attack > cardToEvaluate.Attack)
                        {
                            points += 10;
                            reasons.Add("This card has more attack power than the chosen card. (+10)");
                        }
                        else if (card.Attack == cardToEvaluate.Attack)
                        {
                            points += 5;
                            reasons.Add("This card has the same attack as the chosen card. (+5)");
                        }
                        else
                        {
                            points -= 5;
                            reasons.Add("Unfortunately, this card has less attack then your chosen card. (-5)");
                        }

                        if (card.Health > cardToEvaluate.Health)
                        {
                            points += 10;
                            reasons.Add("This card has more health then the chosen card. (+10)");
                        }
                        else if (card.Health == cardToEvaluate.Health)
                        {
                            points += 5;
                            reasons.Add("This card has the same health as the chosen card. (+5)");
                        }
                        else
                        {
                            points -= 5;
                            reasons.Add("Unfortunately, this card has less health then your chosen card. (-5)");
                        }
                    }
                    else if (card.Type == CardType.WEAPON)
                    {
                        if (card.Durability > cardToEvaluate.Durability)
                        {
                            points += 10;
                            reasons.Add("This card has more durability than the chosen card. (+10)");
                        }
                        else if (card.Durability == cardToEvaluate.Durability)
                        {
                            points += 5;
                            reasons.Add("This card has the same durability than the chosen card. (+5)");
                        }
                        else
                        {
                            points -= 5;
                            reasons.Add("Unfortunately, this card has less durability then your chosen card. (-5)");
                        }


                        if (card.Attack > cardToEvaluate.Attack)
                        {
                            points += 10;
                            reasons.Add("This card has more attack power than the chosen card. (+10)");
                        }
                        else if (card.Attack == cardToEvaluate.Attack)
                        {
                            points += 5;
                            reasons.Add("This card has the same attack as the chosen card. (+5)");
                        }
                        else
                        {
                            points -= 5;
                            reasons.Add("Unfortunately, this card has less attack then your chosen card. (-5)");
                        }
                    }
                    foreach (var mechanic in card.Mechanics)
                    {
                        if (cardToEvaluate.Mechanics.Contains(mechanic))
                        {
                            points += 10;
                            reasons.Add($"This card has the same mechanic '{mechanic}' then your chosen card. (+10)");
                        }
                        else
                        {
                            points += 5;
                            reasons.Add($"This card has a mechanic '{mechanic}' that your chosen card does not possess. (+5)");
                        }
                    }

                    CardSuggestion suggestion = new CardSuggestion
                    {
                        Card = card,
                        Points = points,
                        Reasons = reasons
                    };

                    suggestions.Add(suggestion);
                }
            }

            return suggestions.OrderByDescending(x => x.Points).Take(20).ToList();
        }

        private static int GetOpponentCardUsage(Card card)
        {
            int usage = 0;

            foreach (Game game in RecordedGames)
            {
                usage += game.Opponent.Deck.CardsInDeck.Count(x => x.DbfId == card.DbfId);
            }

            return usage;
        }

        private static int GetUserWinCardUsage(Card card)
        {
            int usage = 0;

            foreach (Game game in RecordedGames)
            {
                if (game.User.Result == "WON")
                {
                    usage += game.User.Deck.CardsInDeck.Count(x => x.DbfId == card.DbfId);
                }
            }

            return usage;
        }
    }
}
