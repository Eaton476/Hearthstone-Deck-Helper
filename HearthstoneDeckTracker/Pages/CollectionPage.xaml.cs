using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HearthDb;
using HearthDb.Deckstrings;
using HearthDb.Enums;
using HearthstoneDeckTracker.Model;
using HearthstoneDeckTracker.Utilities;
using HearthstoneDeckTracker.ViewModel;

namespace HearthstoneDeckTracker.Pages
{
    /// <summary>
    /// Interaction logic for CollectionPage.xaml
    /// </summary>
    public partial class CollectionPage : Page
    {
        private readonly List<DeckListView> _deckListView = new List<DeckListView>();
        private Deck _selectedDeck = null;
        private Card _shownCard;

        public CollectionPage()
        {
            InitializeComponent();
            RefreshDecklist();
            InitialiseClasslist();

            DataGridCurrentDecks.AutoGenerateColumns = true;
            DataGridCurrentDecks.IsReadOnly = true;
            DataGridCurrentDecks.SelectionMode = DataGridSelectionMode.Single;

            DataGridSelectedDeck.AutoGenerateColumns = true;
            DataGridSelectedDeck.IsReadOnly = true;
            DataGridSelectedDeck.SelectionMode = DataGridSelectionMode.Single;
        }

        private void RefreshDecklist()
        {
            Database.CurrentDecks.ForEach(x => _deckListView.Add(new DeckListView(x)));
            DataGridCurrentDecks.ItemsSource = null;
            DataGridCurrentDecks.ItemsSource = _deckListView;
        }

        private async void BtnCardSearch_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtBoxCardSearch.Text))
            {
                string query = TxtBoxCardSearch.Text.ToLower();
                Task<List<Card>> task = new Task<List<Card>>(() => Cards.SearchCardsByNameAsync(query, Locale.enUS));
                task.Start();
                LblSearchingInformation.Visibility = Visibility.Visible;
                List<Card> results = await task;
                LblResults.Content = $"{results.Count} results found.";
                DataGridSearchResults.ItemsSource = results.Select(x => new CardListView(x)).ToList();
                LblSearchingInformation.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("You must provide some text to search for!", "Error!", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ShowCardDetails(CardListView card)
        {
            _shownCard = Cards.GetFromDbfId(card.Dbfid);
            string imageSource = Path.Combine(Config.CardImagesFolder(), $"{card.Dbfid}.png");
            BitmapImage image;
            if (!File.Exists(Path.Combine(Config.CardImagesFolder(), $"{card.Dbfid}.png")))
            {
                imageSource = Path.Combine(Config.CardImagesFolder(), "1688.png");
            }
            image = new BitmapImage(new Uri(imageSource));

            TxtBoxCardName.Text = card.Name;
            TxtBoxCardType.Text = card.Type;
            TxtBoxCardRarity.Text = card.Rarity;
            TxtBoxCardSet.Text = card.Set;
            CardImage.Source = image;
        }

        private void ShowCardDetails(Card card)
        {
            CardListView cardView = new CardListView(card);
            ShowCardDetails(cardView);
        }

        private void DataGridSearchResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridSearchResults.SelectedItem is CardListView card)
            {
                ShowCardDetails(card);
                LblCardInformation.Text = "Card Information - Search";
            }
        }

        private void BtnCreateDeck_Click(object sender, RoutedEventArgs e)
        {
            if (PanelCreateDeck.Visibility == Visibility.Collapsed)
            {
                PanelCreateDeck.Visibility = Visibility.Visible;
            }
            else
            {
                PanelCreateDeck.Visibility = Visibility.Collapsed;
            }
        }

        private void InitialiseClasslist()
        {
            List<Card> classes = Cards.GetHeroClasses();
            ComboSelectClass.ItemsSource = classes;
            ComboSelectClass.DisplayMemberPath = "Name";
            ComboSelectClass.SelectedValuePath = "Name";
            ComboSelectClass.SelectedValue = classes;
        }

        private void BtnConfirmClass_Click(object sender, RoutedEventArgs e)
        {
            Card selectedHero = (Card) ComboSelectClass.SelectedItem;
            string deckName = TxtBoxDeckName.Text;

            if (selectedHero != null && !string.IsNullOrWhiteSpace(deckName))
            {
                Database.CreateNewDeck(selectedHero, deckName);
                ResetDeckCreationPanel();
                RefreshDecklist();
            }
        }

        private void ResetDeckCreationPanel()
        {
            ComboSelectClass.SelectedIndex = 1;
            TxtBoxDeckName.Clear();
            PanelCreateDeck.Visibility = Visibility.Collapsed;
        }

        private void CurrentDecksDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridCurrentDecks.SelectedItem is DeckListView)
            {
                DeckListView view = DataGridCurrentDecks.SelectedItem as DeckListView;;
                _selectedDeck = Database.GetDeckFromCurrentDeckListView(view);
                InitialiseSelectedDeck();
            }
        }

        private void InitialiseSelectedDeck()
        {
            DataGridSelectedDeck.ItemsSource = Database.GetCardListViewDeckFromDeck(_selectedDeck);
            LblSelectedDeck.Text = $"Selected Deck - {_selectedDeck.Name}";
        }

        private void DataGridSelectedDeck_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridSelectedDeck.SelectedItem is CardListViewDeck)
            {
                CardListViewDeck card = DataGridSelectedDeck.SelectedItem as CardListViewDeck;
                Card cardToShow = _selectedDeck.GetCards().Keys.FirstOrDefault(x => x.Name == card.Name);
                ShowCardDetails(cardToShow);
                LblCardInformation.Text = "Card Information - Deck";
            }
        }

        private void BtnAddCardToDeck_Click(object sender, RoutedEventArgs e)
        {
            if (_shownCard != null && _selectedDeck != null)
            {
                _selectedDeck.CardDbfIds.TryGetValue(_shownCard.DbfId, out int count);
                if (count == 0)
                {
                    _selectedDeck.CardDbfIds.Add(_shownCard.DbfId, 1);
                }
                else if (count == 1)
                {
                    if (_shownCard.Rarity != Rarity.LEGENDARY)
                    {
                        _selectedDeck.CardDbfIds[_shownCard.DbfId] = 2;
                    }
                    else
                    {
                        MessageBox.Show("You may not have more than 1 of each legendary in your deck!", "Error",
                            MessageBoxButton.OK, MessageBoxImage.Error);

                        return;
                    }
                }
                else
                {
                    MessageBox.Show("You can only have 2 of each non-legendary card in your deck.", "Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);

                    return;
                }

                RefreshDecklist();
                InitialiseSelectedDeck();
            }
        }
    }
}
