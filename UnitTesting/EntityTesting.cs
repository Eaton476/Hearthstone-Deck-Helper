using HearthDb;
using HearthDb.CardDefs;
using HearthDb.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class EntityTesting
    {
        //Naming Convention: [UnitOfWork__StateUnderTest__ExpectedBehavior] : http://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html

        private Entity _testEntity;

        [TestInitialize]
        public void Initialise()
        {
            _testEntity = new Entity
            {
                CardId = "AT_008",
                DbfId = 2544,
                EntityId = 0,
                Name = "Coldarra Drake",
                Tags = Cards.GetFromDbfId(2544).Entity.Tags
            };
        }

        [TestMethod]
        public void GetTag_GettingTagValueViaEnumId_Success()
        {
            int attackValue = _testEntity.GetTag(GameTag.ATK);

            Assert.IsTrue(attackValue > 0);
        }

        [TestMethod]
        public void GetTag_GettingTagValueViaEnumId_Fail()
        {
            // Entity doesn't have this tag.
            int attackValue = _testEntity.GetTag(GameTag.ADJACENT_BUFF);

            Assert.IsFalse(attackValue > 0);
        }

        [TestMethod]
        public void ToString_Success()
        {
            string entity = _testEntity.ToString();

            Assert.IsInstanceOfType(entity, typeof(string));
        }

        [TestMethod]
        public void GetLocalString_EnUS_Success()
        {
            
        }
    }
}
