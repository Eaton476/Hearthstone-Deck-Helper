using HearthstoneDeckTracker.Tracker;
using System.Windows;
using System.Windows.Controls;
using HearthstoneDeckTracker.Enums;
using HearthstoneDeckTracker.Model;
using HearthstoneDeckTracker.Utilities;
using HearthstoneDeckTracker.ViewModel;

namespace HearthstoneDeckTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly MainWindowViewModel _dataContext = new MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = _dataContext;

            Log.Initialize();
            Database.LoadData();
        
            //API.GetAllCardData();
            //ZoneLogFileReader zoneLogFileReader = new ZoneLogFileReader(Config.HearthstoneLogDirectory(), Config.HearthstoneZoneLogFile());
            //zoneLogFileReader.WatchLogFile();
            //ListViewInteractions.ItemsSource = zoneLogFileReader.Interactions;

			//Log.Initialize();
			//LogFileHandler handler = new LogFileHandler();
			//handler.Start();
        }

        private void CollectionButton_Click(object sender, RoutedEventArgs e)
        {
            _dataContext.CurrentPage = PageNumber.Collection;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            _dataContext.CurrentPage = PageNumber.Home;
        }

        private void HomeScreen_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Database.SaveData();
        }
    }
}
