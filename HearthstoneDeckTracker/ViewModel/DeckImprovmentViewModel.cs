using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HearthDb;
using HearthDb.Deckstrings;
using HearthstoneDeckTracker.Annotations;
using HearthstoneDeckTracker.Model;
using HearthstoneDeckTracker.Utilities;

namespace HearthstoneDeckTracker.ViewModel
{
    public class DeckImprovmentViewModel : INotifyPropertyChanged
    {
        public List<Deck> PlayerCreatedDecks => Database.CurrentDecks;
        public Deck SelectedPlayerDeck { get; set; }
        public ObservableCollection<CardListView> CardsInSelectedDeck { get; set; }
        public Card SelectedCard { get; set; }
        public List<CardSuggestion> CardSuggestions { get; set; }
        public ObservableCollection<CardSuggestionView> CardSuggestionViews { get; set; }
        public CardSuggestion SelectedCardSuggestion { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void UpdateCardsInDeckView()
        {
            if (SelectedPlayerDeck != null)
            {
                CardsInSelectedDeck = new ObservableCollection<CardListView>(SelectedPlayerDeck.CardDbfIds.Select(x => new CardListView(Cards.GetFromDbfId(x.Key))).ToList());
            }
            else
            {
                Log.Error("Player has not selected a deck to update the cards view with.");
            }
        }
    }
}
