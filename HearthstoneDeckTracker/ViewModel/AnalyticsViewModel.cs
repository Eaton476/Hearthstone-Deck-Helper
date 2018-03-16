using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HearthstoneDeckTracker.Annotations;
using HearthstoneDeckTracker.Model;
using LiveCharts;

namespace HearthstoneDeckTracker.ViewModel
{
    public class AnalyticsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };

        public SeriesCollection GameResultSeries => Database.GetGameResultSeries();
    }
}
