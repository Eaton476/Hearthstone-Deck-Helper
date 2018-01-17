using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Resources;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HearthDb;
using HearthDb.Deckstrings;
using HearthDb.Enums;
using HearthstoneDeckTracker.Model;
using HearthstoneDeckTracker.Utilities;
using HearthstoneDeckTracker.ViewModel;
using MessageBox = System.Windows.MessageBox;

namespace HearthstoneDeckTracker.Pages
{
    /// <summary>
    /// Interaction logic for CollectionPage.xaml
    /// </summary>
    public partial class CollectionPage : Page
    {
        private readonly List<DeckListView> _deckListView = new List<DeckListView>();
        private Deck _selectedDeck;
        private Card _shownCard;
        private bool _editDeck = false;
        private string _deckToEdit;

        public CollectionPage()
        {
            InitializeComponent();
            InitialiseClasslist();
            RefreshDecklist();

            DataGridCurrentDecks.AutoGenerateColumns = true;
            DataGridCurrentDecks.IsReadOnly = true;
            DataGridCurrentDecks.SelectionMode = DataGridSelectionMode.Single;

            DataGridSelectedDeck.AutoGenerateColumns = true;
            DataGridSelectedDeck.IsReadOnly = true;
            DataGridSelectedDeck.SelectionMode = DataGridSelectionMode.Single;
        }

        private void RefreshDecklist()
        {
            _deckListView.Clear();
            Database.CurrentDecks.ForEach(x => _deckListView.Add(new DeckListView(x)));
            DataGridCurrentDecks.ItemsSource = _deckListView;
            DataGridCurrentDecks.Items.Refresh();
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

            _editDeck = false;
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
                if (_editDeck)
                {
                    Database.EditDeck(_deckToEdit, deckName, selectedHero);
                }
                else
                {
                    Database.CreateNewDeck(selectedHero, deckName);
                }

                ResetDeckCreationPanel();
                RefreshDecklist();
            }
        }

        private void ResetDeckCreationPanel()
        {
            ComboSelectClass.SelectedIndex = 0;
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
            DataGridSelectedDeck.Items.Refresh();
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
                Deck deckToEdit = Database.CurrentDecks.First(x => x.Name == _selectedDeck.Name);
                deckToEdit.CardDbfIds.TryGetValue(_shownCard.DbfId, out int count);
                if (count == 0)
                {
                    deckToEdit.CardDbfIds.Add(_shownCard.DbfId, 1);
                }
                else if (count == 1)
                {
                    if (_shownCard.Rarity != Rarity.LEGENDARY)
                    {
                        deckToEdit.CardDbfIds[_shownCard.DbfId] = 2;
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

        private void BtnDeleteCard_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridSelectedDeck.SelectedItem is CardListViewDeck)
            {
                CardListViewDeck card = DataGridSelectedDeck.SelectedItem as CardListViewDeck;
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {card.Name}?", "Warning",
                    MessageBoxButton.YesNoCancel);

                if (result == MessageBoxResult.Yes)
                {
                    Deck deckToEdit = Database.CurrentDecks.First(x => x.Name == _selectedDeck.Name);
                    Card cardToRemove = deckToEdit.GetCards().Keys.First(x => x.Name == card.Name);
                    deckToEdit.CardDbfIds.TryGetValue(cardToRemove.DbfId, out int quantity);
                    if (quantity > 1)
                    {
                        deckToEdit.CardDbfIds[cardToRemove.DbfId] = 1;
                    }
                    else
                    {
                        deckToEdit.CardDbfIds.Remove(cardToRemove.DbfId);
                    }

                    RefreshDecklist();
                    InitialiseSelectedDeck();
                }
            }
        }

        private void BtnEditDeck_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridCurrentDecks.SelectedItem is DeckListView)
            {
                if (PanelCreateDeck.Visibility == Visibility.Collapsed)
                {
                    DeckListView view = DataGridCurrentDecks.SelectedItem as DeckListView;
                    Deck deckToEdit = Database.CurrentDecks.First(x => x.Name == view.Name);
                    _deckToEdit = deckToEdit.Name;
                    TxtBoxDeckName.Text = deckToEdit.Name;
                    ComboSelectClass.SelectedItem = deckToEdit.GetHero();
                    _editDeck = true;
                    PanelCreateDeck.Visibility = Visibility.Visible;
                }
                else
                {
                    ResetDeckCreationPanel();
                }
            }
        }

        private void BtnDeleteDeck_Click(object sender, RoutedEventArgs e)
        {
            if (DataGridCurrentDecks.SelectedItem is DeckListView)
            {
                DeckListView view = DataGridCurrentDecks.SelectedItem as DeckListView;
                MessageBoxResult result = MessageBox.Show($"Are you sure you want to delete {view.Name}", "Warning",
                    MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    Database.DeleteDeck(view.Name);
                    RefreshDecklist();
                }
            }
        }
    }
}
