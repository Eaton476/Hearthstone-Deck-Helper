using HearthstoneDeckTracker.Data;
using HearthstoneDeckTracker.Tracker;
using HearthstoneDeckTracker.Utilities;
using System.Windows;
using HearthstoneDeckTracker.Interfaces;

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

            //API.GetAllCardData();
            ZoneLogFileReader zoneLogFileReader = new ZoneLogFileReader(Config.HearthstoneLogDirectory(), Config.HearthstoneZoneLogFile());
            zoneLogFileReader.WatchLogFile();

            ListViewInteractions.ItemsSource = zoneLogFileReader.Interactions;
        }
    }
}
