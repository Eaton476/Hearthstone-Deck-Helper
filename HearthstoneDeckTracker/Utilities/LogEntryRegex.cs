using System.Text.RegularExpressions;

namespace HearthstoneDeckTracker.Utilities
{
	public static class LogEntryRegex
	{
		public static readonly Regex EntityRegex =
				new Regex(
					@"(?=id=(?<id>(\d+)))(?=name=(?<name>(\w+)))?(?=zone=(?<zone>(\w+)))?(?=zonePos=(?<zonePos>(\d+)))?(?=cardId=(?<cardId>(\w+)))?(?=player=(?<player>(\d+)))?(?=type=(?<type>(\w+)))?");
		public static readonly Regex LogEntryStructure = new Regex("^(D|W) (?<timestamp>([\\d:.]+)) (?<line>(.*))$");
		public static readonly Regex BeginBlurRegex = new Regex(@"BeginEffect blur \d => 1");
		public static readonly Regex TagChangeRegex =
			new Regex(@"TAG_CHANGE\ Entity=(?<entity>(.+))\ tag=(?<tag>(\w+))\ value=(?<value>(\w+))");
	    public static readonly Regex PlayerEntityRegex =
	        new Regex(@"Player\ EntityID=(?<id>(\d+))\ PlayerID=(?<playerId>(\d+))\ GameAccountId=(?<gameAccountId>(.+))");
    }
}
