using System.Collections.Generic;
using System.Windows.Controls;
using HearthstoneDeckTracker.Model;
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
    }
}
