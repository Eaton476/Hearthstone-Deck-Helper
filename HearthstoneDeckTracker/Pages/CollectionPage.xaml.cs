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
        private List<CurrentDeckListView> _deckListView = new List<CurrentDeckListView>();

        public CollectionPage()
        {
            InitializeComponent();
            InitialiseDecklist();
        }

        private void InitialiseDecklist()
        {
            Database.CurrentDecks.ForEach(x => _deckListView.Add(new CurrentDeckListView(x)));
            CurrentDecksDataGrid.ItemsSource = _deckListView;
            CurrentDecksDataGrid.AutoGenerateColumns = true;
            CurrentDecksDataGrid.IsReadOnly = true;
            CurrentDecksDataGrid.SelectionMode = DataGridSelectionMode.Single;
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
                DataGridSearchResults.ItemsSource = results.Select(x => new CardSearch(x)).ToList();
                LblSearchingInformation.Visibility = Visibility.Collapsed;
            }
            else
            {
                MessageBox.Show("You must provide some text to search for!", "Error!", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void ShowCardDetails(CardSearch card)
        {
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

        private void DataGridSearchResults_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DataGridSearchResults.SelectedItem is CardSearch card)
            {
                ShowCardDetails(card);
            }
        }

        private void DataGridSearchResults_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
