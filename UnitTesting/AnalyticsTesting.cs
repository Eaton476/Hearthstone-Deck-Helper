using System;
using HearthstoneDeckTracker.Model;
using LiveCharts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class AnalyticsTesting
    {
        [TestInitialize]
        public void Initialise()
        {
            Database.LoadData();
        }

        [TestCleanup]
        public void Cleanup()
        {
            Database.ClearLoadedData();
        }

        [TestMethod]
        public void DatabaseAnalytics_GetGameResultSeries_Success()
        {
            SeriesCollection seriesCollection = Database.GetGameResultSeries();

            Assert.IsNotNull(seriesCollection);
        }

        [TestMethod]
        public void DatabaseAnalytics_GetHeroSelectionSeries_Success()
        {
            SeriesCollection seriesCollection = Database.GetHeroSelectionSeries();

            Assert.IsNotNull(seriesCollection);
        }

        [TestMethod]
        public void DatabaseAnalytics_GetAverageMinionDeaths_Success()
        {
            int average = Database.GetAverageMinionDeaths();

            Assert.IsTrue(average > 0);
        }

        [TestMethod]
        public void DatabaseAnalytics_GetUserCardCostAverageSeries_Success()
        {
            SeriesCollection seriesCollection = Database.GetUserCardCostAverageSeries();

            Assert.IsNotNull(seriesCollection);
        }

        [TestMethod]
        public void DatabaseAnalytics_GetOpponentCardCostAverageSeries_Success()
        {
            SeriesCollection seriesCollection = Database.GetOpponentCardCostAverageSeries();

            Assert.IsNotNull(seriesCollection);
        }

        [TestMethod]
        public void DatabaseAnalytics_GetAverageGameDuration_Success()
        {
            string duration = Database.GetAverageGameDuration();

            Assert.IsNotNull(duration);
        }
    }
}
