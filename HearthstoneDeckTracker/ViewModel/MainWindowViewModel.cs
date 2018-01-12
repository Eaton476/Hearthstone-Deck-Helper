using System.ComponentModel;
using HearthstoneDeckTracker.Enums;

namespace HearthstoneDeckTracker.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private PageNumber _currentPage = PageNumber.Home;

        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };

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
