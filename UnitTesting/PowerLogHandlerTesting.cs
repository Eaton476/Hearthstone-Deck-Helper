using System;
using System.Text.RegularExpressions;
using HearthstoneDeckTracker.Tracker;
using HearthstoneDeckTracker.Tracker.HearthstoneLogHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTesting
{
    [TestClass]
    public class PowerLogHandlerTesting
    {
        //Naming Convention: [UnitOfWork__StateUnderTest__ExpectedBehavior] : http://osherove.com/blog/2005/4/3/naming-standards-for-unit-tests.html

        private string _fullEntityUpdatingExample;
        private string _fullEntityCreatingExample;
        private string _fullEntityUpdatingUnknownExample;
        private string _showEntityUnknownUpdatingExample;
        private string _showEntityKnownUpdatingExample;
        private string _tagChangeShortExample;
        private string _tagChangeLongExample;
        private string _tagChangeLongUnknownExample;
        private PowerLogFileHandler _fileHandler;

        [TestInitialize]
        public void TestInitialise()
        {
            _fileHandler = new PowerLogFileHandler();

            _fullEntityUpdatingExample = "FULL_ENTITY - Updating [entityName=Fireblast id=67 zone=PLAY zonePos=0 cardId=CS2_034 player=2] CardID=CS2_034";
            _fullEntityUpdatingUnknownExample = "FULL_ENTITY - Updating [entityName=UNKNOWN ENTITY [cardType=INVALID] id=4 zone=DECK zonePos=0 cardId= player=1] CardID=";
            _fullEntityCreatingExample = "FULL_ENTITY - Creating ID=12 CardID=CS2_141";
            _showEntityUnknownUpdatingExample =
                "SHOW_ENTITY - Updating Entity=[entityName=UNKNOWN ENTITY [cardType=INVALID] id=12 zone=DECK zonePos=0 cardId= player=1] CardID=LOOT_060";
            _showEntityKnownUpdatingExample = "SHOW_ENTITY - Updating Entity=10 CardID=EX1_565";
            _tagChangeShortExample = "TAG_CHANGE Entity=EatonGaming tag=RESOURCES_USED value=2";
            _tagChangeLongExample = "TAG_CHANGE Entity=[entityName=Bilefin Tidehunter id=63 zone=HAND zonePos=5 cardId=OG_156 player=2] tag=ZONE_POSITION value=1";
            _tagChangeLongUnknownExample = "TAG_CHANGE Entity=[entityName=UNKNOWN ENTITY [cardType=INVALID] id=5 zone=HAND zonePos=3 cardId= player=1] tag=CARD_TARGET value=72";

        }

        [TestMethod]
        public void RegexMatching_FullEntityUpdating_IsTrue()
        {
            Regex regex = LogEntryRegex.CreationRegexUpdating;
            Match match = regex.Match(_fullEntityUpdatingExample);
            var groups = match.Groups;

            Assert.IsTrue(match.Success);
        }

        [TestMethod]
        public void RegexMatching_FullEntityUpdatingUnknown_IsFalse()
        {
            Regex regex = LogEntryRegex.CreationRegexUpdating;
            Match match = regex.Match(_fullEntityUpdatingUnknownExample);
            var groups = match.Groups;

            Assert.IsFalse(match.Success);
        }

        [TestMethod]
        public void LogHandling_FullEntityUpdating_IsTrue()
        {
            LogEntry entry = new LogEntry("", _fullEntityUpdatingExample);
            _fileHandler.Handle(entry);
        }

        [TestMethod]
        public void RegexMatching_FullEntityCreating_IsTrue()
        {
            Regex regex = LogEntryRegex.CreationRegexCreating;
            Match match = regex.Match(_fullEntityCreatingExample);
            var groups = match.Groups;

            Assert.IsTrue(match.Success);
        }

        [TestMethod]
        public void LogHandling_FullEntityCreating_IsTrue()
        {
            LogEntry entry = new LogEntry("", _fullEntityCreatingExample);
            _fileHandler.Handle(entry);
        }

        [TestMethod]
        public void RegexMatching_ShowEntityUpdating_IsTrue()
        {
            Regex regex = LogEntryRegex.UpdatingEntityRegex;
            Match match = regex.Match(_showEntityUnknownUpdatingExample);
            var groups = match.Groups;

            Assert.IsTrue(match.Success);
        }

        [TestMethod]
        public void LogHandling_ShowEntityUpdating_IsTrue()
        {
            LogEntry entry = new LogEntry("", _showEntityUnknownUpdatingExample);
            _fileHandler.Handle(entry);
        }

        [TestMethod]
        public void RegexMatching_ShowKnownEntityUpdating_IsTrue()
        {
            Regex regex = LogEntryRegex.UpdatingEntityRegex;
            Match match = regex.Match(_showEntityKnownUpdatingExample);
            var groups = match.Groups;

            Assert.IsTrue(match.Success);
        }

        [TestMethod]
        public void LogHandling_ShowKnownEntityUpdating_IsTrue()
        {
            LogEntry entry = new LogEntry("", _showEntityKnownUpdatingExample);
            _fileHandler.Handle(entry);
        }

        [TestMethod]
        public void RegexMatching_TagChangeKnownShort_IsTrue()
        {
            Regex regex = LogEntryRegex.TagChangeRegex;
            Match match = regex.Match(_tagChangeShortExample);
            var groups = match.Groups;

            Assert.IsTrue(match.Success);
        }

        [TestMethod]
        public void RegexMatching_TagChangeKnownLong_IsTrue()
        {
            Regex regex = LogEntryRegex.TagChangeRegex;
            Match match = regex.Match(_tagChangeLongExample);
            var groups = match.Groups;

            Assert.IsTrue(match.Success);
        }

        [TestMethod]
        public void RegexMatching_TagChangeUnknownLong_IsTrue()
        {
            Regex regex = LogEntryRegex.TagChangeRegex;
            Match match = regex.Match(_tagChangeLongUnknownExample);
            var groups = match.Groups;

            Assert.IsTrue(match.Success);
        }

        [TestMethod]
        public void LogHandling_TagChangeKnownShort_IsTrue()
        {
            LogEntry entry = new LogEntry("", _tagChangeShortExample);
            _fileHandler.Handle(entry);
        }

        [TestMethod]
        public void LogHandling_TagChangeKnownLong_IsTrue()
        {
            LogEntry entry = new LogEntry("", _tagChangeLongExample);
            _fileHandler.Handle(entry);
        }

        [TestMethod]
        public void LogHandling_TagChangeUnknownLong_IsTrue()
        {
            LogEntry entry = new LogEntry("", _tagChangeLongUnknownExample);
            _fileHandler.Handle(entry);
        }
    }
}
