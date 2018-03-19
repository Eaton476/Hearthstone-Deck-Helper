using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HearthstoneDeckTracker.Model;

namespace HearthstoneDeckTracker.Pages
{
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public partial class Settings : Page
	{
		public Settings()
		{
			InitializeComponent();
		}

        private void BtnOutputEntities_Click(object sender, RoutedEventArgs e)
        {
            Database.CurrentGame.OutputEntitiesToLog();
        }

        private void BtnLoadEntities_Click(object sender, RoutedEventArgs e)
        {
            Database.CurrentGame.LoadTestEntitiesFromXml();
        }

        private void BtnSaveEntities_Click(object sender, RoutedEventArgs e)
        {
            Database.CurrentGame.SaveTestEntitiesToXml();
        }

        private void BtnImportDecks_Click(object sender, RoutedEventArgs e)
        {
            Database.ImportDecksFromHearthstone();
        }
    }
}
