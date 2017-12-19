using System.Text.RegularExpressions;

namespace HearthstoneDeckTracker.Utilities
{
	public static class LogTemplates
	{
		public static readonly Regex EntityRegex =
				new Regex(
					@"(?=id=(?<id>(\d+)))(?=name=(?<name>(\w+)))?(?=zone=(?<zone>(\w+)))?(?=zonePos=(?<zonePos>(\d+)))?(?=cardId=(?<cardId>(\w+)))?(?=player=(?<player>(\d+)))?(?=type=(?<type>(\w+)))?");

		public static readonly Regex LogEntryStructure = new Regex("^(D|W) (?<timestamp>([\\d:.]+)) (?<line>(.*))$");
	}
}
