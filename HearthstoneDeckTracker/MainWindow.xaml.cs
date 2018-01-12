using HearthstoneDeckTracker.Tracker;
using System.Windows;
using System.Windows.Controls;
using HearthstoneDeckTracker.Utilities;
using HearthstoneDeckTracker.ViewModel;

namespace HearthstoneDeckTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainWindowViewModel();

            //API.GetAllCardData();
            //ZoneLogFileReader zoneLogFileReader = new ZoneLogFileReader(Config.HearthstoneLogDirectory(), Config.HearthstoneZoneLogFile());
            //zoneLogFileReader.WatchLogFile();
            //ListViewInteractions.ItemsSource = zoneLogFileReader.Interactions;

			//Log.Initialize();
			//LogFileHandler handler = new LogFileHandler();
			//handler.Start();
        }
    }
}
