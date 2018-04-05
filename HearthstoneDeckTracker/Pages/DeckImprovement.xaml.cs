using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using HearthDb;
using HearthDb.Enums;
using HearthstoneDeckTracker.Model;
using HearthstoneDeckTracker.Utilities;
using HearthstoneDeckTracker.ViewModel;

namespace HearthstoneDeckTracker.Pages
{
    /// <summary>
    /// Interaction logic for DeckImprovement.xaml
    /// </summary>
    public partial class DeckImprovement : Page
    {
        private readonly DeckImprovmentViewModel _viewModel = new DeckImprovmentViewModel();

        public DeckImprovement()
        {
            InitializeComponent();

            this.DataContext = _viewModel;
        }

        private void ComboBoxCreatedDecks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDeckView();
        }

        private void UpdateDeckView()
        {
            _viewModel.UpdateCardsInDeckView();
            DataGridCardsInDeck.ItemsSource = _viewModel.CardsInSelectedDeck;
            DataGridCardsInDeck.Items.Refresh();
        }

        private void DataGridCardsInDeck_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridCardsInDeck.SelectedItem is CardListView card)
            {
                _viewModel.SelectedCard = Cards.GetFromDbfId(card.Dbfid);
                ShowCardDetails();
            }
        }

        private void ShowCardDetails()
        {
            string imageSource = Path.Combine(Config.CardImagesFolder(), $"{_viewModel.SelectedCard.DbfId}.png");
            if (!File.Exists(Path.Combine(Config.CardImagesFolder(), $"{_viewModel.SelectedCard.DbfId}.png")))
            {
                imageSource = Path.Combine(Config.CardImagesFolder(), "1688.png");
            }
            var image = new BitmapImage(new Uri(imageSource));

            try
            {
                TxtBoxCardName.Text = _viewModel.SelectedCard.Name;
                TxtBoxCardType.Text = Enum.GetName(typeof(CardType), _viewModel.SelectedCard.Type) ?? throw new InvalidOperationException();
                TxtBoxCardRarity.Text = Enum.GetName(typeof(Rarity), _viewModel.SelectedCard.Rarity) ?? throw new InvalidOperationException();
                TxtBoxCardSet.Text = Enum.GetName(typeof(CardSet), _viewModel.SelectedCard.Set) ?? throw new InvalidOperationException();
                CardImage.Source = image;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private async void BtnGenerateResults_Click(object sender, RoutedEventArgs e)
        {
            Task<List<CardSuggestion>> task = new Task<List<CardSuggestion>>(() => Database.GenerateCardSuggestions(_viewModel.SelectedCard));
            task.Start();
            TxtBlockResults.Text = "Generating results...";
            _viewModel.CardSuggestions = await task;
            TxtBlockResults.Text =
                $"{_viewModel.CardSuggestions.Count} results generated, only the top 20 are shown below.";
            _viewModel.CardSuggestionViews = new ObservableCollection<CardSuggestionView>(_viewModel.CardSuggestions.Select(x => new CardSuggestionView(x)));

            DataGridCardChoices.ItemsSource = _viewModel.CardSuggestionViews;
            DataGridCardChoices.Items.Refresh();
        }

        private void DataGridCardChoices_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridCardChoices.SelectedItem is CardSuggestionView card)
            {
                _viewModel.SelectedCardSuggestion = _viewModel.CardSuggestions.First(x => x.Card.DbfId == card.Dbfid);
                ShowGeneratedCardDetails();
            }
        }

        private void ShowGeneratedCardDetails()
        {
            string imageSource = Path.Combine(Config.CardImagesFolder(), $"{_viewModel.SelectedCardSuggestion.Card.DbfId}.png");
            if (!File.Exists(Path.Combine(Config.CardImagesFolder(), $"{_viewModel.SelectedCardSuggestion.Card.DbfId}.png")))
            {
                imageSource = Path.Combine(Config.CardImagesFolder(), "1688.png");
            }
            var image = new BitmapImage(new Uri(imageSource));

            try
            {
                TxtBoxGenCardName.Text = _viewModel.SelectedCardSuggestion.Card.Name;
                TxtBoxGenCardType.Text = Enum.GetName(typeof(CardType), _viewModel.SelectedCardSuggestion.Card.Type) ?? throw new InvalidOperationException();
                TxtBoxGenCardRarity.Text = Enum.GetName(typeof(Rarity), _viewModel.SelectedCardSuggestion.Card.Rarity) ?? throw new InvalidOperationException();
                TxtBoxCardResultsInformation.Text = _viewModel.SelectedCardSuggestion.ReasonsToString();
                GenCardImage.Source = image;
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private void BtnSwapCard_Click(object sender, RoutedEventArgs e)
        {
            int dbfidToRemove = _viewModel.SelectedCard.DbfId;
            int dbfidToAdd = _viewModel.SelectedCardSuggestion.Card.DbfId;
            var deck = Database.CurrentDecks.FirstOrDefault(x => x == _viewModel.SelectedPlayerDeck);

            if (deck != null)
            {
                int amount = deck.CardDbfIds[dbfidToRemove];

                deck.CardDbfIds.Remove(dbfidToRemove);
                deck.CardDbfIds.Add(dbfidToAdd, amount);

                UpdateDeckView();

                MessageBox.Show(
                    "The generated alternative card you have chosen has replaced the initial card you selected in your deck, remember to make the correct change in the game client.",
                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                ClearCardDisplay();
            }
        }

        private void ClearCardDisplay()
        {
            TxtBoxCardName.Clear();
            TxtBoxCardType.Clear();
            TxtBoxCardRarity.Clear();
            TxtBoxCardSet.Clear();
            CardImage.Source = null;
        }
    }
}
