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
using HearthstoneDeckTracker.ViewModel;

namespace HearthstoneDeckTracker.Pages
{
    /// <summary>
    /// Interaction logic for Analytics.xaml
    /// </summary>
    public partial class Analytics : Page
    {
        private readonly AnalyticsViewModel _viewModel = new AnalyticsViewModel();

        public Analytics()
        {
            InitializeComponent();

            this.DataContext = _viewModel;
        }
    }
}
