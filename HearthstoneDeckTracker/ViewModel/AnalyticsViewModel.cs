using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HearthMirror.Objects;
using HearthstoneDeckTracker.Annotations;
using HearthstoneDeckTracker.Model;
using LiveCharts;

namespace HearthstoneDeckTracker.ViewModel
{
    public class AnalyticsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, args) => { };
        public SeriesCollection GameResultSeries => Database.GetGameResultSeries();
        public SeriesCollection HeroSelectionSeries => Database.GetHeroSelectionSeries();
        public int AverageMinionDeathsValue => Database.GetAverageMinionDeaths();
        public SeriesCollection UserCardCostAverageSeries => Database.GetUserCardCostAverageSeries();
        public SeriesCollection OpponentCardCostAverageSeries => Database.GetOpponentCardCostAverageSeries();
        public string NumberOfGames => Database.RecordedGames.Count.ToString();
        public string AverageGameTime => Database.GetAverageGameDuration();
    }
}
