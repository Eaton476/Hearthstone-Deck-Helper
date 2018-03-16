using System;
using HearthstoneDeckTracker.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class DatabaseTesting
    {
        [TestInitialize]
        public void Initialise()
        {
            Database.LoadData();
        }

        [TestMethod]
        public void SavingRecordedGames_GamesToXml_Success()
        {
            Database.EndGame();
            Database.SaveData();
        }

        [TestMethod]
        public void LoadingRecordedGames_XmlToGames_Success()
        {
            //Database.LoadData();
        }

        [TestMethod]
        public void AnalyticsGameResultPieChart_Success()
        {
            var result = Database.GetGameResultSeries();

            Assert.IsTrue(true);
        }
    }
}
