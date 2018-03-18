using System;
using System.Collections.Generic;
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
    }
}
