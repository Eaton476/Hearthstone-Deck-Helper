﻿using HearthstoneDeckTracker.Tracker;
using System.Windows;
using HearthstoneDeckTracker.Utilities;

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
            //ZoneLogFileReader zoneLogFileReader = new ZoneLogFileReader(Config.HearthstoneLogDirectory(), Config.HearthstoneZoneLogFile());
            //zoneLogFileReader.WatchLogFile();
            //ListViewInteractions.ItemsSource = zoneLogFileReader.Interactions;

			Log.Initialize();
			LogFileHandler handler = new LogFileHandler();
			handler.Start();
        }
    }
}