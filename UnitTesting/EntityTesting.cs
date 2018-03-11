using System;
using HearthDb.Enums;
using HearthstoneDeckTracker.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HearthstoneDeckTracker.Utilities.Converters;

namespace UnitTesting
{
    [TestClass]
    public class EntityTesting
    {
        //Naming Convention: [UnitOfWork__StateUnderTest__ExpectedBehavior] : http://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html

        [TestInitialize]
        public void Initialise()
        {
            Database.CurrentGame.LoadTestEntitiesFromXml();
        }

        [TestMethod]
        public void CleaningEntities_EntitiesSavedFromGame_Sucess()
        {
            Database.CurrentGame.ProcessEntities();
        }

        [TestMethod]
        public void OutputtingEntities_EntitiesSavedFromGame_Sucess()
        {
            foreach (var entity in Database.CurrentGame.Entities.Values)
            {
                foreach (var tag in entity.Tags)
                {
                    GameTag gameTag = GameTagConverter.ParseEnum<GameTag>(tag.EnumId.ToString());
                    string t = tag.ToString();
                }
            }
        }

        [TestMethod]
        public void ProcessingEntities_EntitiesSavedFromGame_Sucess()
        {
            Database.CurrentGame.ProcessEntities();
            Assert.IsTrue(true);
        }
    }
}
