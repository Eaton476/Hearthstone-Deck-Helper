using System.Collections.Generic;
using System.Threading.Tasks;
using HearthstoneDeckTracker.Tracker;
using System.Windows;
using System.Windows.Controls;
using HearthDb;
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
        private LogFileHandler _fileHandler = new LogFileHandler();

        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = _dataContext;

            Log.Initialize();
            Database.LoadData();
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

        private void btnToggleMonitoring_Click(object sender, RoutedEventArgs e)
        {
            if (_fileHandler.GetRunning())
            {
                _fileHandler.StopAsync();
                lblMonitoringStatus.Content = "Not Monitoring Gameplay.";
            }
            else
            {
                _fileHandler.Start();
                lblMonitoringStatus.Content = "Monitoring Gameplay.";
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            _dataContext.CurrentPage = PageNumber.Settings;
        }

        private void AnalyticsButton_Click(object sender, RoutedEventArgs e)
        {
            _dataContext.CurrentPage = PageNumber.Analytics;
        }

        private void SuggestionButton_Click(object sender, RoutedEventArgs e)
        {
            _dataContext.CurrentPage = PageNumber.DeckImprovement;
        }

        //private void ComboBoxHeroSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    Database.UserHero = _dataContext.SelectedHero;
        //    //Config.SetDefaultSelectedHero(Database.UserHero.Id);
        //}
    }
}
