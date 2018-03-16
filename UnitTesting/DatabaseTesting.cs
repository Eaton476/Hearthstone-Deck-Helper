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
            Database.CurrentGame.LoadTestEntitiesFromXml();
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
            Database.LoadData();
        }
    }
}
