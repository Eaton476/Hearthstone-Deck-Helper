using System.Collections.Generic;
using HearthMirror;
using HearthMirror.Enums;
using HearthMirror.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class HearthMirrorTesting
    {
        [TestMethod]
        public void HearthMirrorReflection_GetMatchInfo_Success()
        {
            if (Status.GetStatus().MirrorStatus != MirrorStatus.ProcNotFound)
            {
                //THIS TEST WILL ONLY WORK WHEN A GAME IS IN PROGRESS
                if (!Reflection.IsInMainMenu())
                {
                    MatchInfo info = Reflection.GetMatchInfo();

                    Assert.IsInstanceOfType(info, typeof(MatchInfo));
                }
                else
                {
                    Assert.Inconclusive("A game of Hearthstone must be in progress for the test to pass.");
                }
            }
            else
            {
                Assert.Inconclusive("Hearthstone isn't running, so this test cannot be ran successfully.");
            }
        }

        [TestMethod]
        public void HearthMirrorReflection_GetAccountId_Success()
        {
            if (Status.GetStatus().MirrorStatus != MirrorStatus.ProcNotFound)
            {
                AccountId id = Reflection.GetAccountId();

                Assert.IsInstanceOfType(id, typeof(AccountId));
            }
            else
            {
                Assert.Inconclusive("Hearthstone isn't running, so this test cannot be ran successfully.");
            }
        }

        [TestMethod]
        public void HearthMirrorReflection_GetCollection_Success()
        {
            if (Status.GetStatus().MirrorStatus != MirrorStatus.ProcNotFound)
            {
                Collection collection = Reflection.GetFullCollection();

                Assert.IsInstanceOfType(collection, typeof(Collection));
            }
            else
            {
                Assert.Inconclusive("Hearthstone isn't running, so this test cannot be ran successfully.");
            }
        }

        [TestMethod]
        public void HearthMirrorStatus_GetStatus_Success()
        {
            Status status = Status.GetStatus();

            Assert.IsNotNull(status);
        }

        [TestMethod]
        public void HearthMirrorReflection_GetDecks_Success()
        {
            if (Status.GetStatus().MirrorStatus != MirrorStatus.ProcNotFound)
            {
                List<Deck> decks = Reflection.GetDecks();

                Assert.IsNotNull(decks);
            }
            else
            {
                Assert.Inconclusive("Hearthstone isn't running, so this test cannot be ran successfully.");
            }
        }
    }
}
