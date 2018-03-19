using System;
using System.Collections.Generic;
using System.ComponentModel;
using HearthDb;
using HearthstoneDeckTracker.Annotations;
using HearthstoneDeckTracker.Enums;
using HearthstoneDeckTracker.Model;
using HearthstoneDeckTracker.Utilities;

namespace HearthstoneDeckTracker.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private PageNumber _currentPage = PageNumber.Home;

        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };
        public List<Card> HeroChoices => Cards.GetHeroClasses();
        public Card SelectedHero { get; set; }

        public PageNumber CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage == value)
                {
                    return;
                }
                _currentPage = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CurrentPage"));
            }
        }
    }
}
